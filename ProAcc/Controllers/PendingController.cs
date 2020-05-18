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

    public class PendingController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: Pending
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PendingMaster pend)
        {
            var name = db.PendingMasters.Where(p => p.PendingName == pend.PendingName).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                pend.Cre_on = DateTime.Now;
                pend.Cre_By = Guid.Parse(Session["loginid"].ToString());
                pend.isActive = true;

                db.PendingMasters.Add(pend);
                db.SaveChanges();
            }
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckPending(string name, int? id)
        {
            if(id!=null)
            {
                var em = db.PendingMasters.Where(p => p.PendingName == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
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
                var em = db.PendingMasters.Where(p => p.PendingName == name).Where(x => x.isActive == true).ToList();
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
        public ActionResult GetPendingById(int id)
        {
            var pend = db.PendingMasters.Find(id);
            PendingMaster master = new PendingMaster();
            master.Id = id;
            master.PendingName = pend.PendingName;
            master.Cre_on = pend.Cre_on;
            master.Cre_By = pend.Cre_By;
            return Json(master, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(PendingMaster model)
        {
            model.Cre_on = DateTime.Now;
            //model.Cre_By = Guid.Parse(Session["loginid"].ToString());
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
            PendingMaster pendmaster = db.PendingMasters.Find(id);
            pendmaster.isActive = false;
            pendmaster.IsDeleted = true;
            db.Entry(pendmaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }
    }
}