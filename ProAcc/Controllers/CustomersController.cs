using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProAcc.BL.Model;
using ProACC_DB;
using PagedList;
using PagedList.Mvc;
using ProAcc.BL;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();

        // GET: Customers

        public ActionResult LinkPage()
        {
            return View();
        }
        public ActionResult Index()
        {
            var customers = db.Customers
                .Where(a => a.isActive == true)
                .OrderByDescending(x => x.Cre_on).ToList();
            //.Where(x => x.Name.StartsWith(search) || search == null).ToList(); //.ToPagedList(i ?? 1, 5);
            return View(customers);
            //ViewBag.customersIndex = db.Customers.Where(x => x.isActive == true).OrderByDescending(x => x.Cre_on).ToList();
            //return View(ViewBag.customersIndex);
        }
        // GET: Customers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        public JsonResult CheckCustomersNameAvailability(string namedata,Guid? id)
        {
            if(id!=null)
            {
                var SearchDt = db.Customers.Where(x => x.Company_Name == namedata).Where(x=>x.Customer_ID!=id).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = db.Customers.Where(x => x.Company_Name == namedata).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.IndustrySector = db.IndustrySectors.Where(x => x.IsActive == true);
            ViewBag.customersIndex = db.Customers.Where(x => x.isActive == true).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult GetCustomers()
        {
            var list = db.Customers.Where(x => x.isActive == true).ToList();
            return PartialView("_CustomerIndex", list);
        }
        
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                var name = db.Customers.Where(p => p.Company_Name == customer.Company_Name).Where(x => x.isActive == true).ToList();
                if (name.Count==0)
                {
                    customer.Customer_ID = Guid.NewGuid();
                    customer.isActive = true;
                    customer.Cre_on = DateTime.UtcNow;
                    customer.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return Json("success");                    
                }
                else
                {
                    return Json("error");
                }                
                
            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, Url);
                throw;
            }
            
        }

        public ActionResult GetCustomerById(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var Data = db.Customers.Find(id);
            ViewBag.IndustrySector = db.IndustrySectors.Where(x => x.IsActive == true);
            Customer cust = new Customer();
            cust.Customer_ID = Data.Customer_ID;
            cust.Company_Name = Data.Company_Name;
            cust.Contact = Data.Contact;
            cust.Phone = Data.Phone;
            cust.Email = Data.Email;
            return View(customer);
            //return Json(cust, JsonRequestBehavior.AllowGet);
        }

        // GET: Customers/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var val = db.User_Type.Where(a => a.isActive == true);
           
        //    return View(customer);
        //}

     
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            var name = db.Customers.Where(p => p.Company_Name == customer.Company_Name).Where(x => x.Customer_ID != customer.Customer_ID).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                customer.Modified_On = DateTime.UtcNow;
                //customer.Cre_on = DateTime.Now;
                customer.Modified_by = Guid.Parse(Session["loginid"].ToString());
                customer.isActive = true;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            
            var del = (from a in db.Customers
                       join b in db.Projects
                       on a.Customer_ID equals b.Customer_Id
                       where a.Customer_ID==id && a.isActive == true && b.isActive == true
                       select b).ToList();

            if(del.Count!=0)
            {
                return Json("fail");
            }
            else
            {
                Customer customer = db.Customers.Find(id);
                if (customer.Customer_ID == id)
                {
                    customer.isActive = false;
                    customer.IsDeleted = true;
                    db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("success");
            }

            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
