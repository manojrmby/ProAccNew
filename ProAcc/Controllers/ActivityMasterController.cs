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
    public class ActivityMasterController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: ActivityMaster
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetActivityById(Guid id)
        {
            var activity = db.ActivityMasters.Find(id);
            ActivityMaster master = new ActivityMaster();
            master.Id = id;
            master.Activity = activity.Activity;
            master.ApplicationAreaID = activity.ApplicationAreaID;
            master.PhaseID = activity.PhaseID;
            return Json(master, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(ActivityMaster acmaster)
        {
            var name = db.ActivityMasters.Where(p => p.Activity == acmaster.Activity).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                acmaster.Cre_on = DateTime.Now;
                acmaster.Cre_By = Guid.Parse(Session["loginid"].ToString());
                acmaster.isActive = true;
                acmaster.Id = Guid.NewGuid();

                db.ActivityMasters.Add(acmaster);
                db.SaveChanges();
            }
            return Json("success");
        }
        [HttpPost]
        public ActionResult Edit(ActivityMaster model)
        {
            model.Cre_on = DateTime.Now;
            model.Modified_by= Guid.Parse(Session["loginid"].ToString());
            model.Modified_On = DateTime.Now;
            model.isActive = true;
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckActivity(string name, Guid? id)
        {
            if(id!=null)
            {
                var em = db.ActivityMasters.Where(p => p.Activity == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
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
                var em = db.ActivityMasters.Where(p => p.Activity == name).Where(x => x.isActive == true).ToList();
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

        [HttpPost]
        public ActionResult Delete(Guid id)
        {

            ActivityMaster acmaster = db.ActivityMasters.Find(id);
            acmaster.isActive = false;
            acmaster.IsDeleted = true;
            db.Entry(acmaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }
    }
}