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

        //[HttpGet]
        //public ActionResult GetActivities()
        //{
        //    var Activitylist = db.ActivityMasters.Where(x => x.isActive == true).OrderBy(a=>a.Sequence_Num).ToList();
        //    return PartialView("_ActivityCreationIndex", Activitylist);
        //}
        [HttpGet]
        public ActionResult GetActivities()
        {
            var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.Sequence_Num != null).OrderBy(a => a.Sequence_Num).ToList();
            return PartialView("_ActivityCreationIndex", Activitylist);
        }
        [HttpGet]
        public ActionResult GetAllTask(int id)
        {
            var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.PhaseID == id&&x.Sequence_Num!=null).Select(p=> new{ p.Activity_ID,p.Task}).ToList();
            return Json(Activitylist, JsonRequestBehavior.AllowGet);           
        }
        [HttpGet]
        public ActionResult LastTask(int id)
        {
            var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.PhaseID == id&&x.Sequence_Num!=null).Select(p=> new{ p.Activity_ID,p.Task,p.Sequence_Num}).ToList();
            return Json(Activitylist, JsonRequestBehavior.AllowGet);           
        }
        [HttpPost]
        public ActionResult SaveAccordingToSequence(int LatestTaskId, int LastTaskId, int type)
        {
            int? seqNumber = 0;
            if (LastTaskId != 0)
            {
                var lastTask = db.ActivityMasters.Where(p => p.Activity_ID == LastTaskId).FirstOrDefault();
                seqNumber = Convert.ToInt32(lastTask.Sequence_Num);
            }

            int? nextSeqNumber = null;
            if (seqNumber == 0)
            {
                if (type == 5)
                    nextSeqNumber = 1;
                else if (type == 2)
                    nextSeqNumber = 1001;
                else if (type == 3)
                    nextSeqNumber = 2001;
                else if (type == 4)
                    nextSeqNumber = 3001;
            }
            else
            {
                nextSeqNumber = seqNumber + 1;
            }
            var latestTask = db.ActivityMasters.Where(p => p.Activity_ID == LatestTaskId).FirstOrDefault();
            latestTask.Sequence_Num = nextSeqNumber;
            db.Entry(latestTask).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            List<int> ids = db.ActivityMasters.Where(p => p.Activity_ID > LastTaskId && p.Activity_ID != LatestTaskId && p.PhaseID == type).Select(p => p.Activity_ID).ToList();
            int? tid = nextSeqNumber;
            foreach (var id in ids)
            {
                var task = db.ActivityMasters.Where(p => p.Activity_ID == id).FirstOrDefault();
                task.Sequence_Num = tid + 1;
                tid = Convert.ToInt32(task.Sequence_Num);
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json("success");
        }
        //[HttpPost]
        //public ActionResult SaveAccordingToSequence(int LatestTaskId,int LastTaskId,int type)
        //{           
        //    var lastTask = db.ActivityMasters.Where(p=>p.Activity_ID==LastTaskId).FirstOrDefault();
        //    int? seqNumber = Convert.ToInt32(lastTask.Sequence_Num);
        //    int? nextSeqNumber = null;
        //    if (seqNumber == 0)
        //    {
        //        if (type == 5)
        //            nextSeqNumber = 1;
        //        else if (type == 2)
        //            nextSeqNumber = 1001;
        //        else if (type == 3)
        //            nextSeqNumber = 2001;
        //        else if (type == 4)
        //            nextSeqNumber = 3001;
        //    }
        //    else
        //    {
        //        nextSeqNumber = seqNumber + 1;
        //    }
        //    var latestTask = db.ActivityMasters.Where(p => p.Activity_ID == LatestTaskId).FirstOrDefault();
        //    latestTask.Sequence_Num = nextSeqNumber;
        //    db.Entry(latestTask).State = System.Data.Entity.EntityState.Modified;
        //    db.SaveChanges();
        //    List<int> ids = db.ActivityMasters.Where(p => p.Activity_ID > LastTaskId && p.Activity_ID != LatestTaskId && p.PhaseID==type).Select(p => p.Activity_ID).ToList();
        //    int? tid = nextSeqNumber;
        //    foreach(var id in ids)
        //    {
        //        var task = db.ActivityMasters.Where(p => p.Activity_ID == id).FirstOrDefault();
        //        task.Sequence_Num = tid+1;
        //        tid = Convert.ToInt32(task.Sequence_Num);
        //        db.Entry(task).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    return Json("success");
        //}
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
                    return Json(activityMaster.Activity_ID,JsonRequestBehavior.AllowGet);
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
        public ActionResult GetActivityCreationById(int? id)
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
            //var Activity = db.ActivityMasters.Find(id);
            var Activity = db.ActivityMasters.Where(x => x.isActive == true && x.Activity_ID == id).Select(p => new { p.Activity_ID, p.Task, p.ApplicationArea, p.PhaseID, p.RoleID,p.Cre_on,p.Cre_By,p.Sequence_Num }).FirstOrDefault();
            //var Activity = db.ActivityMasters.Where(x => x.isActive == true && x.Activity_ID == id).Select(p => new { p.Activity_ID, p.Task,p.ApplicationArea,p.PhaseID,p.RoleID });

            return Json(Activity, JsonRequestBehavior.AllowGet);
            //var list = JsonConvert.SerializeObject(Activity, Formatting.None,
            //new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});

            //return Content(list, "application/json");
        }

        [HttpPost]
        public ActionResult Edit(ActivityMaster model)
        {
            var name = db.ActivityMasters.Where(p => p.Task == model.Task).Where(x => x.Activity_ID != model.Activity_ID).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                model.Modified_On = DateTime.Now;
                model.Cre_on = DateTime.Now;
                model.Modified_by = Guid.Parse(Session["loginid"].ToString());
                model.isActive = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json(model.Activity_ID, JsonRequestBehavior.AllowGet);
                //return Json("success");
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
        [HttpGet]
        public ActionResult Testing()
        {
            return View();
        }
        
    }
}