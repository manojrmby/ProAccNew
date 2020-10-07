using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class ActivityController : Controller
    {
        Base _Base = new Base();
        ProAccEntities db = new ProAccEntities();
        // GET: Activity
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            ViewBag.TaskId = db.TaskType1.ToList();
            ViewBag.ParallelId = db.ParallelTypes.ToList();
            ViewBag.Phase = db.PhaseMasters.Where(x => x.isActive == true).OrderBy(x=>x.Id);
            var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
            var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
            ViewBag.Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!= adminRoleId && x.RoleId != pmRoleId).OrderBy(x=>x.RoleName);
            ViewBag.Block = db.Buldingblocks.Where(x => x.isActive == true).OrderBy(x => x.Block_Name);
            ViewBag.ApplicationArea = db.ApplicationAreaMasters.Where(x => x.isActive == true).OrderBy(x => x.ApplicationArea);
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
            //var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.Sequence_Num != null).OrderBy(a => a.Sequence_Num).ToList();
            List<ActivityMasterModel> Activitylist = _Base.Sp_GetActivityMasterData(); //Sp_GetIssueTrackData();
            ViewBag.AMM = Activitylist;
            return PartialView("_ActivityCreationIndex");
        }
        [HttpGet]
        public ActionResult GetAllTaskByParallelType(int id,Guid Parallel_Id)
        {
           var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.PhaseID == id && x.Sequence_Num != null&&x.Parallel_Id== Parallel_Id).OrderBy(a => a.Sequence_Num).Select(p => new { p.Activity_ID, p.Task }).ToList();
           return Json(Activitylist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAllTask(int id)
        {
           var Activitylist = db.ActivityMasters.Where(x => x.isActive == true && x.PhaseID == id && x.Sequence_Num != null).OrderBy(a => a.Sequence_Num).Select(p => new { p.Activity_ID, p.Task }).ToList();
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
                if (type == 1)
                    nextSeqNumber = 1;
                else if (type == 2)
                    nextSeqNumber = 10001;
                else if (type == 3)
                    nextSeqNumber = 20001;
                else if (type == 4)
                    nextSeqNumber = 30001;
                else if (type == 5)
                    nextSeqNumber = 40001;
            }
            else
            {
                nextSeqNumber = seqNumber + 1;
            }
            var latestTask = db.ActivityMasters.Where(p => p.Activity_ID == LatestTaskId).FirstOrDefault();
            latestTask.Sequence_Num = nextSeqNumber;
            latestTask.isActive = true;
            latestTask.Modified_On= DateTime.UtcNow;
            latestTask.Modified_by= Guid.Parse(Session["loginid"].ToString());
            db.Entry(latestTask).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            var ids = db.ActivityMasters.Where(p => p.Activity_ID > LastTaskId && p.Activity_ID != LatestTaskId && p.PhaseID == type).OrderBy(p=>p.Sequence_Num).ToList();
            int? tid = nextSeqNumber;
            foreach (var id in ids)
            {
                var task = db.ActivityMasters.Where(p => p.Activity_ID == id.Activity_ID).FirstOrDefault();
                task.Sequence_Num = tid + 1;
                tid = Convert.ToInt32(task.Sequence_Num);
                task.Modified_On = DateTime.UtcNow;
                task.Modified_by = Guid.Parse(Session["loginid"].ToString());
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
                    activityMaster.isActive = false;
                    activityMaster.Cre_on = DateTime.UtcNow;
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

            var Activity = _Base.Sp_GetActivityCreationById(id);
                //db.ActivityMasters.Where(x => x.isActive == true && x.Activity_ID == id).Select(p => new { p.Activity_ID, p.Task, p.ApplicationAreaID, p.PhaseID, p.RoleID,p.Cre_on,p.Cre_By,p.Sequence_Num ,p.BuildingBlock_id,p.EST_hours}).FirstOrDefault();

            return Json(Activity, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public ActionResult Edit(ActivityMaster model)
        {
            var name = db.ActivityMasters.Where(p => p.Task == model.Task).Where(x => x.Activity_ID != model.Activity_ID).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                model.Modified_On = DateTime.UtcNow;
                model.Cre_on = DateTime.UtcNow;
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
                activity.Modified_On = DateTime.UtcNow;
                activity.Modified_by = Guid.Parse(Session["loginid"].ToString());
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


        [HttpPost]
        public ActionResult ActivityCreate(Activity_Master activityMaster)
        {
            try
            {
                activityMaster.Modified_by= Guid.Parse(Session["loginid"].ToString());
                _Base.Activity_Master_Add_Update(activityMaster);
                //var name = db.ActivityMasters.Where(p => p.Task == activityMaster.Task).Where(x => x.isActive == true).ToList();
                //if (name.Count == 0)
                //{
                //    //activityMaster.Activity_ID = Guid.NewGuid();
                //    activityMaster.isActive = false;
                //    activityMaster.Cre_on = DateTime.UtcNow;
                //    activityMaster.Cre_By = Guid.Parse(Session["loginid"].ToString());
                //    db.ActivityMasters.Add(activityMaster);
                //    db.SaveChanges();
                return Json(activityMaster.Activity_ID, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //return Json("error");
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}