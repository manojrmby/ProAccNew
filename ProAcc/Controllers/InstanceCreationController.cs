using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProACC_DB;
using System.Data.Entity;
using ProAcc.BL;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class InstanceCreationController : Controller
    {
        ProAccEntities db = new ProAccEntities();
        Base _Base = new Base();
        // GET: InstanceCreation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            //ViewBag.project = db.Projects.Where(x => x.isActive == true);
            ViewBag.project = (from e in db.Projects
                               join cu in db.Customers on e.Customer_Id equals cu.Customer_ID
                               where e.isActive == true && cu.isActive == true
                               select e).OrderBy(x => x.Project_Name).ToList();
            return View();
        }

        public JsonResult CheckInstanceNameAvailability(string namedata, Guid? id)
        {
            if (id != null)
            {
                var SearchDt = db.Instances.Where(x => x.InstaceName == namedata).Where(x => x.Instance_id != id).Where(x => x.isActive == true).FirstOrDefault();
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
                var SearchDt = db.Instances.Where(x => x.InstaceName == namedata).Where(x => x.isActive == true).FirstOrDefault();
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

        public JsonResult CheckInstanceProjectAvailability(string namedata, Guid? project)
        {
            var SearchDt = db.Instances.Where(x => x.InstaceName == namedata && x.Project_ID == project).Where(x => x.isActive == true).FirstOrDefault();
            if (SearchDt != null)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetInstances()
        {
            //var InstanceList = db.Instances.Where(x => x.isActive == true).ToList();
            var InstanceList = (from e in db.Instances
                                join c in db.Projects on e.Project_ID equals c.Project_Id
                                join cu in db.Customers on c.Customer_Id equals cu.Customer_ID
                                where e.isActive == true && c.isActive == true && cu.isActive == true
                                select e).OrderByDescending(x => x.Cre_on).ToList();
            return PartialView("_InstanceIndex", InstanceList);
        }

        public ActionResult GetInstancesByProject(Guid id)
        {
            var InstanceList = (from e in db.Instances
                                join c in db.Projects on e.Project_ID equals c.Project_Id
                                join cu in db.Customers on c.Customer_Id equals cu.Customer_ID
                                where e.isActive == true && c.isActive == true && cu.isActive == true && e.Project_ID == id
                                select new { e.InstaceName, e.Instance_id }).ToList().OrderBy(x=>x.InstaceName);
            return Json(InstanceList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(Instance instance)
        {
            LogHelper _log = new LogHelper();
            try
            {
                var name = db.Instances.Where(p => p.InstaceName == instance.InstaceName && p.Project_ID == instance.Project_ID).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    instance.Instance_id = Guid.NewGuid();
                    instance.isActive = true;
                    instance.Cre_on = DateTime.UtcNow;
                    instance.LastUpdated_Dt = DateTime.UtcNow; ;
                    instance.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    bool status = _Base.Sp_InstanceCreate(instance);
                    if (status == true)
                    {
                        return Json("success");
                    }
                    else
                    {
                        return Json("error");
                    }
                }
                else
                {
                    return Json("error");
                }

            }
            catch (Exception ex)
            {
                _log.createLog(ex);
                throw;
            }
        }


        public ActionResult CreateClone(String InstaceName, Guid Project_ID, Guid Previous_Instance)
        {
            Boolean Result = false;
            Instance Data = new Instance();
            Data.InstaceName = InstaceName;
            Data.Project_ID = Project_ID;
            Data.Cre_By = Guid.Parse(Session["loginid"].ToString());
            Result = _Base.Sp_InstanceClone(Data, Previous_Instance);
            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetInstCreationById(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instance instance = db.Instances.Find(id);
            if (instance == null)
            {
                return HttpNotFound();
            }
            var Instance = db.Instances.Find(id);

            var list = JsonConvert.SerializeObject(Instance, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });

            return Content(list, "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Instance model)
        {
            var name = db.Instances.Where(p => p.InstaceName == model.InstaceName).Where(x => x.Instance_id != model.Instance_id).Where(x => x.isActive == true).ToList();
            if (name.Count == 0)
            {
                model.Modified_On = DateTime.UtcNow;
                model.LastUpdated_Dt = DateTime.UtcNow;
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
            bool result = false;

            var del = (from a in db.Instances
                       join b in db.ProjectMonitors
                       on a.Instance_id equals b.InstanceID
                       where b.InstanceID == id && a.isActive == true && b.isActive == true
                       //select new { V = b.StatusId != 4 } ).ToList();
                       select new { V = (b.StatusId != 2 && b.StatusId != 4 && b.StatusId != 5) }).ToList();

            foreach (var i in del)
            {
                if (i.V == false)
                {
                    result = true;
                    break;
                }
            }
            if (result == true)
            {
                return Json("fail");
            }
            else
            {
                var ids = db.ProjectMonitors.Where(x => x.InstanceID == id && x.isActive == true).ToList();
                foreach (var pmid in ids)
                {
                    var task = db.ProjectMonitors.Where(p => p.InstanceID == pmid.InstanceID && p.Id == pmid.Id).FirstOrDefault();
                    task.isActive = false;
                    task.IsDeleted = true;
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                }

                Instance instance = db.Instances.Find(id);
                if (instance.Instance_id == id)
                {
                    instance.isActive = false;
                    instance.IsDeleted = true;
                    instance.Modified_On = DateTime.UtcNow;
                    instance.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    db.Entry(instance).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("success");
            }

        }
    }
}