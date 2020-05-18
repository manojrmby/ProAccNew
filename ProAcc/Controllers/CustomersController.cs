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
        public ActionResult Index()
        {

            var customers = db.Customers
                .Where(a => a.isActive == true)
                .OrderByDescending(x => x.Cre_on).ToList();
            //.Where(x => x.Name.StartsWith(search) || search == null).ToList(); //.ToPagedList(i ?? 1, 5);
            return View(customers);
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

        public JsonResult CheckCustomersNameAvailability(string namedata)
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

        // GET: Customers/Create
        public ActionResult Create()
        {
            var val = db.User_Type.Where(a=>a.isActive).Where(a => a.UserTypeID ==1 || a.UserTypeID == 2);
            ViewBag.UserType = new SelectList(val, "UserTypeID", "UserType");
            var Team = db.RoleMasters.Where(x => x.isActive == true).Where(a => a.RoleId != 1).ToList();
            ViewBag.Role = new SelectList(Team, "RoleId", "RoleName");
            //var status = db.leadStatus_Master.Where(a => a.isActive == true);
            //ViewBag.LeadStatus = new SelectList(status, "Id", "StatusName");
            //ViewBag.Id = new SelectList(db.Projects, "Id", "Accuracy");
            CreateViewModel model = new CreateViewModel();
            model.Customer = new Customer();
            model.User = new UserMaster();
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                //    if (customer.UserName != null && customer.Password != null)
                //    {
                //        customer.Id = Guid.NewGuid();
                //        customer.Cre_on = DateTime.Now;
                //        customer.Cre_By = Guid.Parse(Session["loginid"].ToString());
                //        customer.UserTypeID = 3;
                //        customer.isActive = true;
                //        db.Customers.Add(customer);
                //        db.SaveChanges();
                //        return RedirectToAction("Index", "Customers");
                //    }
                //    else
                //    {
                //        ViewBag.UserTypeID = new SelectList(db.User_Master, "Id", "UserType", customer.UserTypeID);
                //        ViewBag.Id = new SelectList(db.Projects, "Id", "Accuracy", customer.Id);
                //        ViewBag.LeadStatus = new SelectList(db.leadStatus_Master, "Id", "StatusName", customer.Id);
                //        ViewBag.Message = true;
                //        return View();
                //    }
                //}

                //ViewBag.UserTypeID = new SelectList(db.User_Master.Where(a => a.isActive == true), "Id", "UserType", customer.UserTypeID);
                //ViewBag.Id = new SelectList(db.Projects.Where(a => a.isActive == true), "Id", "Accuracy", customer.Id);
                //ViewBag.LeadStatus = new SelectList(db.leadStatus_Master.Where(a => a.isActive == true), "Id", "StatusName", customer.Id);
                CreateViewModel model = new CreateViewModel();
                model.User = new UserMaster();
                model.Customer = customer;
                return View(model);
            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, Url);
                throw;
            }
            
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(Guid? id)
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
            var val = db.User_Type.Where(a => a.isActive == true);
           // ViewBag.UserTypeID = new SelectList(val, "Id", "UserType", customer.Use);
            //ViewBag.Id = new SelectList(db.Projects, "Id", "Accuracy", customer.Id);
            //var status = db.leadStatus_Master.Where(a => a.isActive == true);
            //ViewBag.LeadStatus = new SelectList(status, "Id", "StatusName", customer.Id);
            return View(customer);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Modified_On = DateTime.Now;
                //customer.Cre_on = DateTime.Now;
                //customer.UserTypeID = 3;
                customer.Modified_by= Guid.Parse(Session["loginid"].ToString());
                customer.isActive = true;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.UserTypeID = new SelectList(db.User_Master, "Id", "UserType", customer.UserTypeID);
           // ViewBag.Id = new SelectList(db.Projects, "Id", "Accuracy", customer.Id);
            //var status = db.leadStatus_Master.Where(a => a.isActive == true);
           // ViewBag.LeadStatus = new SelectList(status, "Id", "StatusName", customer.Id);
            return View(customer);

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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Customer customer = db.Customers.Find(id);
            //db.Customers.Remove(customer);
            if(customer.Customer_ID==id)
            {
                customer.isActive = false;
                customer.IsDeleted = true;
                db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
