using Newtonsoft.Json;
using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProAcc.Controllers
{
    public class ProjectCreationController : Controller
    {
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();
        // GET: ProjectCreation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Customer = db.Customers.Where(x => x.isActive == true).ToList();
            ViewBag.Projdetails = db.Projects.Where(x => x.isActive == true).ToList();
            ViewBag.Scenario = db.ScenarioMasters.Where(x => x.isActive == true).ToList();
            ViewBag.ProjectManager = db.UserMasters.Where(x => x.UserTypeID == 4 && x.isActive == true).ToList();
            
            return View();
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            try
            {
                var Projlist = (from e in db.Projects
                                join c in db.Customers on e.Customer_Id equals c.Customer_ID
                                where e.isActive == true && c.isActive == true
                                select e).OrderByDescending(x => x.Cre_on).ToList();
                return PartialView("_ProjectCreationIndex", Projlist);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetProjects" + Url);
                throw;
            }

           
        }

        public JsonResult CheckProjectNameAvailability(string namedata, Guid? id)
        {
            if (id != null)
            {
                var SearchDt = db.Projects.Where(x => x.Project_Name == namedata).Where(x => x.Project_Id != id).Where(x => x.isActive == true).FirstOrDefault();
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
                var SearchDt = db.Projects.Where(x => x.Project_Name == namedata).Where(x => x.isActive == true).FirstOrDefault();
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
        public ActionResult Create(Project project)
        {
            try
            {
                var name = db.Projects.Where(p => p.Project_Name == project.Project_Name).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    project.Project_Id = Guid.NewGuid();
                    project.isActive = true;
                    project.Cre_on = DateTime.UtcNow;
                    project.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.Projects.Add(project);
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }                
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetProjects" + Url);
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetProjCreationById(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Project project = db.Projects.Find(id);
                if (project == null)
                {
                    return HttpNotFound();
                }
                var Project = db.Projects.Where(x => x.isActive == true && x.Project_Id == id).Select(
                    p => new {
                        p.Project_Id,
                        p.Project_Name,
                        p.Description,
                        p.Customer_Id,
                        p.ProjectManager_Id,
                        p.ScenarioId,
                        p.Cre_on,
                        p.Cre_By
                    }).FirstOrDefault();
                return Json(Project, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetProjCreationById" + Url);
                throw;
            }
        }

        [HttpPost]
        public ActionResult Edit(Project model)
        {
            try
            {

                var name = db.Projects.Where(p => p.Project_Name == model.Project_Name).Where(x => x.Project_Id != model.Project_Id).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    model.Modified_On = DateTime.UtcNow;
                    model.Cre_on = DateTime.UtcNow;
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
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->Edit Post" + Url);
                throw;
            }
        }


        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var del = (from a in db.Projects
                           join b in db.Instances
                           on a.Project_Id equals b.Project_ID
                           where a.Project_Id == id && a.isActive == true && b.isActive == true
                           select b
                    ).ToList();
                if (del.Count != 0)
                {
                    return Json("fail");
                }
                else
                {
                    Project project = db.Projects.Find(id);
                    if (project.Project_Id == id)
                    {
                        project.isActive = false;
                        project.IsDeleted = true;
                        project.Modified_by = Guid.Parse(Session["loginid"].ToString());
                        project.Modified_On = DateTime.UtcNow;
                        db.Entry(project).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json("success");
                }
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->Delete Project Post" + Url);
                throw;
            }

           
           
        }


        public ActionResult PMTaskCreation()
        {
            ViewBag.TaskCat = db.PMTaskCategories.Where(x => x.isActive == true).ToList();
            return View();

        }

        #region PMTask

        [HttpPost]
        public ActionResult PMTaskCreate(PMTaskMaster PMTask)
        {
            try
            {
                var name = db.PMTaskMasters.Where(p => p.PMTaskName == PMTask.PMTaskName).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    PMTask.PMTaskId = Guid.NewGuid();
                    PMTask.isActive = true;
                    PMTask.Cre_on = DateTime.UtcNow;
                    PMTask.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.PMTaskMasters.Add(PMTask);
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->PMTaskCreate" + Url);
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetPMTask()
        {
            List<ProACC_DB.PMTaskMaster> PMTasklist=null;
            try
            {
                PMTasklist = db.PMTaskMasters.Where(x => x.isActive == true).OrderByDescending(x => x.Cre_on).ToList();
                //(from e in db.PMTaskMasters
                //                where e.isActive == true 
                //                select e).OrderByDescending(x => x.Cre_on).ToList();
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->PMTaskCreate" + ex.Message.ToString());
                throw;
            }

            return PartialView("_PMTaskIndex", PMTasklist);
        }


        [HttpGet]
        public ActionResult GetPMtaskById(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PMTaskMaster PMTask = db.PMTaskMasters.Find(id);
                if (PMTask == null)
                {
                    return HttpNotFound();
                }
                var PMTasks = db.PMTaskMasters.Where(x => x.isActive == true && x.PMTaskId == id).Select(
                    p => new {
                        p.PMTaskId,
                        p.PMTaskName,
                        p.PMTaskCategoryID,
                        p.Cre_on,
                        p.Cre_By
                    }).FirstOrDefault();
                return Json(PMTasks, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetPMtaskById" + Url);
                throw;
            }

            
        }


        [HttpPost]
        public ActionResult PMTaskEdit(PMTaskMaster PMTask)
        {
            try
            {
                var name = db.PMTaskMasters.Where(p => p.PMTaskName == PMTask.PMTaskName).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    PMTask.Modified_On = DateTime.UtcNow;
                    PMTask.Cre_on = DateTime.UtcNow;
                    PMTask.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    PMTask.isActive = true;
                    db.Entry(PMTask).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->PMTaskEdit" + Url);
                throw;
            }
        }

        [HttpPost]
        public ActionResult PMTaskDelete(Guid id)
        {
            try
            {
                PMTaskMaster PMTask = db.PMTaskMasters.Find(id);
                if (PMTask.PMTaskId == id)
                {
                    PMTask.isActive = false;
                    PMTask.IsDeleted = true;
                    db.Entry(PMTask).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("success");
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->PMTaskCreate" + Url);
                throw;
            }
        }

        public JsonResult CheckPMTaskAvailability(string namedata, int? id)
        {
            var SearchDt = db.PMTaskMasters.Where(x => x.PMTaskName == namedata).Where(x => x.PMTaskCategoryID == id).Where(x => x.isActive == true).FirstOrDefault();
            if (SearchDt != null)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion



    }
}