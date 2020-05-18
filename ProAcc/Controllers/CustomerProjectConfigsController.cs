using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProACC_DB;
using PagedList;
using PagedList.Mvc;
using ProAcc.BL;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant")]
    public class CustomerProjectConfigsController : Controller
    {
        private ProAccEntities db = new ProAccEntities();
        Guid AdminUser = Guid.Parse("42DC1071-CAAE-4585-AB73-9ADCBE85FDD5");
        // GET: CustomerProjectConfigs
        public ActionResult Index()
        {
            var customerProjectConfigs = db.CustomerProjectConfigs.Where(a => a.isActive == true)
                .OrderByDescending(x => x.Cre_on).ToList();
            //.Where(x => x.ProjectName.StartsWith(search) || search == null).ToPagedList(i ?? 1, 5);
            // var customerProjectConfigs = db.CustomerProjectConfigs.Include(c => c.Consultant).Include(c => c.Customer).Where(a=>a.isActive==true);
            return View(customerProjectConfigs);
        }

        // GET: CustomerProjectConfigs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProjectConfig customerProjectConfig = db.CustomerProjectConfigs.Find(id);
            if (customerProjectConfig == null)
            {
                return HttpNotFound();
            }
            return View(customerProjectConfig);
        }

        // GET: CustomerProjectConfigs/Create
        public ActionResult Create()
        {
            
            var val = db.Consultants.Where(a => a.isActive == true && a.Id != AdminUser);
            ViewBag.ConsultantID = new SelectList(val, "Id", "Name");
            var val1 = db.Customers.Where(a => a.isActive == true);
            ViewBag.CustomerID = new SelectList(val1, "Id", "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerProjectConfig customerProjectConfig)
        {
            if (ModelState.IsValid)
            {
                if (customerProjectConfig.ProjectName != null)
                {
                    customerProjectConfig.Id = Guid.NewGuid();
                    customerProjectConfig.Cre_on = DateTime.Now;
                    customerProjectConfig.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    customerProjectConfig.LastUpdated_Dt = DateTime.Now;
                    customerProjectConfig.isActive = true;

                    var ID = from k in db.CustomerProjectConfigs where k.CustomerID == customerProjectConfig.CustomerID && k.ProjectName == customerProjectConfig.ProjectName select k.Id;
                    if (ID.Count() > 0)
                    {
                        var valid = db.Consultants.Where(a => a.isActive == true);
                        ViewBag.ConsultantID = new SelectList(valid, "Id", "Name", customerProjectConfig.ConsultantID);
                        var valid1 = db.Customers.Where(a => a.isActive == true);
                        ViewBag.CustomerID = new SelectList(valid1, "Id", "Name", customerProjectConfig.CustomerID);
                        ViewBag.Me = true;
                        return View();
                    }

                    db.CustomerProjectConfigs.Add(customerProjectConfig);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var valid = db.Consultants.Where(a => a.isActive == true);
                    ViewBag.ConsultantID = new SelectList(valid, "Id", "Name", customerProjectConfig.ConsultantID);
                    var valid1 = db.Customers.Where(a => a.isActive == true);
                    ViewBag.CustomerID = new SelectList(valid1, "Id", "Name", customerProjectConfig.CustomerID);
                    ViewBag.Message = true;
                    return View();
                }
            }

            var val = db.Consultants.Where(a => a.isActive == true && a.Id != AdminUser);
            ViewBag.ConsultantID = new SelectList(val, "Id", "Name", customerProjectConfig.ConsultantID);
            var val1 = db.Customers.Where(a => a.isActive == true);
            ViewBag.CustomerID = new SelectList(val1, "Id", "Name", customerProjectConfig.CustomerID);
            return View(customerProjectConfig);
        }

        public JsonResult CheckProjectNameAvailability(string projdata)
        {
            System.Threading.Thread.Sleep(100);
            var SearchDt = db.CustomerProjectConfigs.Where(x => x.ProjectName == projdata).Where(x => x.isActive == true).FirstOrDefault();
            if (SearchDt != null)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomerProjectConfigs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProjectConfig customerProjectConfig = db.CustomerProjectConfigs.Find(id);
            if (customerProjectConfig == null)
            {
                return HttpNotFound();
            }
            var val = db.Consultants.Where(a => a.isActive == true && a.Id != AdminUser);
            ViewBag.ConsultantID = new SelectList(val, "Id", "Name", customerProjectConfig.ConsultantID);
            var val1 = db.Customers.Where(a => a.isActive == true);
            ViewBag.CustomerID = new SelectList(val1, "Id", "Name", customerProjectConfig.CustomerID);
            return View(customerProjectConfig);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerProjectConfig customerProjectConfig)
        {
           
            if (ModelState.IsValid)
            {
                customerProjectConfig.Modified_On = DateTime.Now;
                customerProjectConfig.Modified_by= Guid.Parse(Session["loginid"].ToString());
                customerProjectConfig.isActive = true;
                db.Entry(customerProjectConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var val = db.Consultants.Where(a => a.isActive == true && a.Id != AdminUser);
            ViewBag.ConsultantID = new SelectList(val, "Id", "Name", customerProjectConfig.ConsultantID);
            var val1 = db.Customers.Where(a => a.isActive == true);
            ViewBag.CustomerID = new SelectList(val1, "Id", "Name", customerProjectConfig.CustomerID);
            return View(customerProjectConfig);
        }

        // GET: CustomerProjectConfigs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerProjectConfig customerProjectConfig = db.CustomerProjectConfigs.Find(id);
            if (customerProjectConfig == null)
            {
                return HttpNotFound();
            }
            return View(customerProjectConfig);
        }

        // POST: CustomerProjectConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CustomerProjectConfig customerProjectConfig = db.CustomerProjectConfigs.Find(id);
            if(customerProjectConfig.Id==id)
            {
                customerProjectConfig.isActive = false;
                customerProjectConfig.IsDeleted = true;
                db.Entry(customerProjectConfig).State = System.Data.Entity.EntityState.Modified;
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
