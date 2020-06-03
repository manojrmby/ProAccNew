using Newtonsoft.Json;
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
        // GET: ProjectCreation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Customer = db.Customers.Where(x => x.isActive == true).ToList();
            ViewBag.Projdetails = db.Projects.Where(x => x.isActive == true).ToList();
            ViewBag.ProjectManager = db.UserMasters.Where(x => x.UserTypeID == 4 && x.isActive == true).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            //var Projlist = db.Projects.Where(x => x.isActive == true).ToList();
            var Projlist = (from e in db.Projects
                            join c in db.Customers on e.Customer_Id equals c.Customer_ID
                         where e.isActive == true && c.isActive==true
                         select e).OrderByDescending(x => x.Cre_on).ToList();
            return PartialView("_ProjectCreationIndex", Projlist);
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
            catch (Exception)
            {                
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetProjCreationById(Guid? id)
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
            //var Project = db.Projects.Find(id);
            var Project = db.Projects.Where(x => x.isActive == true && x.Project_Id == id).Select(p => new { p.Project_Id, p.Project_Name, p.Description, p.Customer_Id,p.ProjectManager_Id, p.Cre_on, p.Cre_By}).FirstOrDefault();
            
            return Json(Project, JsonRequestBehavior.AllowGet);
            //var list = JsonConvert.SerializeObject(Project, Formatting.None,
            //new JsonSerializerSettings()
            //{
            //    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            //});

            //return Content(list, "application/json");   
        }

        [HttpPost]
        public ActionResult Edit(Project model)
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


        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            Project project = db.Projects.Find(id);
            if (project.Project_Id == id)
            {
                project.isActive = false;
                project.IsDeleted = true;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json("success");
        }
    }
}