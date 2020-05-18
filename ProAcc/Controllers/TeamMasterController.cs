﻿using ProAcc.BL;
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

    public class TeamMasterController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        // GET: TeamMaster
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TeamMaster team)
        {
            var name = db.TeamMasters.Where(p => p.TeamName == team.TeamName).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                team.Cre_on = DateTime.Now;
                team.Cre_By = Guid.Parse(Session["loginid"].ToString());
                team.isActive = true;

                db.TeamMasters.Add(team);
                db.SaveChanges();
            }
            return Json("success");
        }

        [HttpGet]
        public ActionResult CheckTeam(string name,int? id)
        {
            if(id!=null)
            {
                var em = db.TeamMasters.Where(p => p.TeamName == name).Where(x => x.Id != id).Where(x => x.isActive == true).ToList();
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
                var em = db.TeamMasters.Where(p => p.TeamName == name).Where(x => x.isActive == true).ToList();
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
        public ActionResult GetTeamById(int id)
        {
            var team = db.TeamMasters.Find(id);
            TeamMaster master = new TeamMaster();
            master.Id = id;
            master.TeamName = team.TeamName;
            master.Cre_on = team.Cre_on;
            master.Cre_By = team.Cre_By;
            return Json(master, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(TeamMaster model)
        {
            model.Cre_on = DateTime.Now;
            model.Cre_By= Guid.Parse(Session["loginid"].ToString());
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
            TeamMaster tmaster = db.TeamMasters.Find(id);
            tmaster.isActive = false;
            tmaster.IsDeleted = true;
            db.Entry(tmaster).State = EntityState.Modified;
            db.SaveChanges();

            return Json("success");
        }
    }
}