using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProACC_DB;

namespace ProAcc.Controllers
{
    public class ActivityController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: Activity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.Phase = db.PhaseMasters.Where(x => x.isActive == true);
            ViewBag.Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!=1);
            return View();
        }

        [HttpGet]
        public ActionResult GetActivities()
        {
            var Activitylist = db.ActivityMasters.Where(x => x.isActive == true).ToList();
            return PartialView("_ActivityCreationIndex", Activitylist);
        }

        public JsonResult CheckTaskAvailability(string namedata, int? id)
        {
            if (id != null)
            {
                var SearchDt = db.ActivityMasters.Where(x => x.Task == namedata).Where(x => x.Activity_ID != id).Where(x => x.isActive == true).FirstOrDefault();
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
                var SearchDt = db.ActivityMasters.Where(x => x.Task == namedata).Where(x => x.isActive == true).FirstOrDefault();
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

        [HttpPost]
        public ActionResult Create(ActivityMaster activityMaster)
        {
            try
            {
                var name = db.ActivityMasters.Where(p => p.Task == activityMaster.Task).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    //activityMaster.Activity_ID = Guid.NewGuid();
                    activityMaster.isActive = true;
                    activityMaster.Cre_on = DateTime.UtcNow.Date;
                    activityMaster.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.ActivityMasters.Add(activityMaster);
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
                
            }
            catch (Exception)
            {
               
                throw;
            }

        }

        [HttpGet]
        public ActionResult GetActivityCreationById(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityMaster activity = db.ActivityMasters.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            var Activity = db.ActivityMasters.Find(id);

            var list = JsonConvert.SerializeObject(Activity, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }

        [HttpPost]
        public ActionResult Edit(ActivityMaster model)
        {
            var name = db.ActivityMasters.Where(p => p.Task == model.Task).Where(x => x.Activity_ID != model.Activity_ID).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                model.Modified_On = DateTime.Now;
                model.Modified_by = Guid.Parse(Session["loginid"].ToString());
                model.isActive = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            ActivityMaster activity = db.ActivityMasters.Find(id);
            if (activity.Activity_ID == id)
            {
                activity.isActive = false;
                activity.IsDeleted = true;
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json("success");
        }
        
    }
}