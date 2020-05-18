using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant")]
    public class ApplicationAreaController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: ApplicationArea
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ApplicationAreaMaster applicationa)
        {
            applicationa.Cre_on = DateTime.Now;
            applicationa.Cre_By = Guid.Parse(Session["loginid"].ToString());
            applicationa.isActive = true;

            db.ApplicationAreaMasters.Add(applicationa);
            db.SaveChanges();
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckApplicationArea(string name, int? id)
        {
            if(id!=null)
            {
                var em = db.ApplicationAreaMasters.Where(p => p.ApplicationArea == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
                if (em.Count > 0)
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
                var em = db.ApplicationAreaMasters.Where(p => p.ApplicationArea == name).Where(x => x.isActive == true).ToList();
                if (em.Count > 0)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
           
        }

        [HttpGet]
        public ActionResult GetAppById(int id)
        {
            var team = db.ApplicationAreaMasters.Find(id);
            ApplicationAreaMaster master = new ApplicationAreaMaster();
            master.Id = id;
            master.ApplicationArea = team.ApplicationArea;
            master.Cre_on = team.Cre_on;
            master.Cre_By = team.Cre_By;
            return Json(master, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationAreaMaster model)
        {
            model.Cre_on = DateTime.Now;
            model.Cre_By = Guid.Parse(Session["loginid"].ToString());
            model.Modified_On = DateTime.Now;
            model.Modified_by = Guid.Parse(Session["loginid"].ToString());
            model.isActive = true;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json("success");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ApplicationAreaMaster amaster = db.ApplicationAreaMasters.Find(id);
            amaster.isActive = false;
            amaster.IsDeleted = true;
            db.Entry(amaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }
    }
}