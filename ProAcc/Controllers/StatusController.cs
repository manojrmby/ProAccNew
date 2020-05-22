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

    public class StatusController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: Status
        public ActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult Create(StatusMaster status)
        {
            var name = db.StatusMasters.Where(p => p.StatusName == status.StatusName).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                status.Cre_on = DateTime.Now;
                status.Cre_By = Guid.Parse(Session["loginid"].ToString());
                status.isActive = true;

                db.StatusMasters.Add(status);
                db.SaveChanges();
            }
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckStatus(string name, int? id)
        {
            if(id!=null)
            {
                var em = db.StatusMasters.Where(p => p.StatusName == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
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
                var em = db.StatusMasters.Where(p => p.StatusName == name).Where(x => x.isActive == true).ToList();
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
        public ActionResult GetStatusById(int id)
        {
            var team = db.StatusMasters.Find(id);
            StatusMaster master = new StatusMaster();
            master.Id = id;
            master.StatusName = team.StatusName;
            master.Cre_on = team.Cre_on;
            master.Cre_By = team.Cre_By;
            return Json(master, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(StatusMaster model)
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
            StatusMaster stamaster = db.StatusMasters.Find(id);
            stamaster.isActive = false;
            stamaster.IsDeleted = true;
            db.Entry(stamaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }
    }
}