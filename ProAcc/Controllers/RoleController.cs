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
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRolesList()
        {
            List<ProACC_DB.RoleMaster> RoleList = null;
            try
            {
                var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
                var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
                RoleList = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != adminRoleId && x.RoleId != pmRoleId).OrderByDescending(x => x.Cre_on).ToList();
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->GetRolesList" + ex.Message.ToString());
            }

            
            return PartialView("_RoleIndex", RoleList);
        }

        [HttpGet]
        public ActionResult CheckRole(string name, int? id)
        {
            if (id != null)
            {
                var em = db.RoleMasters.Where(p => p.RoleName == name).Where(x => x.RoleId != id).Where(x => x.isActive == true).ToList();
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
                var em = db.RoleMasters.Where(p => p.RoleName == name).Where(x => x.isActive == true).ToList();
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
        public ActionResult Create(RoleMaster Role)
        {
            try
            {
                var name = db.RoleMasters.Where(p => p.RoleName == Role.RoleName).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    Role.Cre_on = DateTime.UtcNow;
                    Role.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    Role.isActive = true;

                    db.RoleMasters.Add(Role);
                    db.SaveChanges();
                }
                return Json("success");
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->Role Create Post" + Url);
                throw;
            }

        }
        

        [HttpGet]
        public ActionResult GetRoleById(int id)
        {
            try
            {
                var team = db.RoleMasters.Find(id);
                RoleMaster master = new RoleMaster();
                master.RoleId = id;
                master.RoleName = team.RoleName;
                master.Cre_on = team.Cre_on;
                master.Cre_By = team.Cre_By;
                TempData["Cre_on"] = team.Cre_on;
                return Json(master, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetRoleById" + Url);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(RoleMaster model)
        {
            try
            {
                model.Cre_on = Convert.ToDateTime(TempData["Cre_on"]);
                //model.Cre_By = Guid.Parse(Session["loginid"].ToString());
                model.Modified_On = DateTime.UtcNow;
                model.Modified_by = Guid.Parse(Session["loginid"].ToString());
                model.isActive = true;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return Json("success");
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->Role Edit Post" + Url);
                throw;
            }

           
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                RoleMaster tmaster = db.RoleMasters.Find(id);
                tmaster.isActive = false;
                tmaster.IsDeleted = true;
                db.Entry(tmaster).State = EntityState.Modified;
                db.SaveChanges();

                return Json("success");
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->Role Delete Post" + Url);
                throw;
            }
        }
    }
}