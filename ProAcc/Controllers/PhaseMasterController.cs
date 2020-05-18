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
    public class PhaseMasterController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: PhaseMaster
        public ActionResult Index()
        {
            ViewBag.phasedetails = db.PhaseMasters.Where(x => x.isActive == true).ToList();
            ViewBag.ApplicationAreadetails = db.ApplicationAreaMasters.Where(x => x.isActive == true).ToList();
            ViewBag.Activitydetails = db.ActivityMasters.Where(x => x.isActive == true).ToList();
            ViewBag.Pendingdetails= db.PendingMasters.Where(x => x.isActive == true).ToList();
            ViewBag.Teamdetails= db.TeamMasters.Where(x => x.isActive == true && x.Id!=1).ToList();
            ViewBag.Statusdetails=db.StatusMasters.Where(x => x.isActive == true).ToList();
            return View();
        }

        //[HttpGet]
        //public ActionResult PhaseIndex()
        //{   
        //    List<PhaseMaster> pm = db.PhaseMasters.Where(x => x.isActive == true).ToList();
        //    var obj = new { data = pm };
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PhaseMaster phase)
        {
            var name= db.PhaseMasters.Where(p => p.PhaseName == phase.PhaseName).Where(x => x.isActive == true).ToList();
            if(name.Count==0)
            {
                phase.Cre_on = DateTime.Now;
                phase.Cre_By = Guid.Parse(Session["loginid"].ToString());
                phase.isActive = true;

                db.PhaseMasters.Add(phase);
                db.SaveChanges();
            }
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckPhase(string name, int? id)
        {
            
            if (id!=null)
            {
               var em = db.PhaseMasters.Where(p => p.PhaseName == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
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
               var em = db.PhaseMasters.Where(p => p.PhaseName == name).Where(x => x.isActive == true).ToList();
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
        public ActionResult GetPhaseById(int id)
        {
            var phase = db.PhaseMasters.Find(id);
            PhaseMaster master = new PhaseMaster();
            master.Id = id;
            master.PhaseName = phase.PhaseName;
            master.Cre_on = phase.Cre_on;
            master.Cre_By = phase.Cre_By;
            return Json(master, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(PhaseMaster model)
        {
            model.Cre_on = DateTime.Now;
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
            PhaseMaster pmaster = db.PhaseMasters.Find(id);
            pmaster.isActive = false;
            pmaster.IsDeleted = true;
            db.Entry(pmaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }

    }
}