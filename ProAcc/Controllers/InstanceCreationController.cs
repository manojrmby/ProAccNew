using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProACC_DB;
using System.Data.Entity;

namespace ProAcc.Controllers
{
    public class InstanceCreationController : Controller
    {
        ProAccEntities db = new ProAccEntities();
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
                               select e).ToList();
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
                            where c.isActive == true && cu.isActive==true
                            select e).ToList();
            return PartialView("_InstanceIndex", InstanceList);
        }

        [HttpPost]
        public ActionResult Create(Instance instance)
        {
            try
            {
                var name = db.Instances.Where(p => p.InstaceName == instance.InstaceName && p.Project_ID == instance.Project_ID).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    instance.Instance_id = Guid.NewGuid();
                    instance.isActive = true;
                    instance.Cre_on = DateTime.Now.Date;
                    instance.LastUpdated_Dt = DateTime.Now.Date;
                    instance.Cre_By = Guid.Parse(Session["loginid"].ToString());

                    db.Instances.Add(instance);
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
                
            }
            catch 
            {
                throw;
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
                model.Modified_On = DateTime.Now;
                model.LastUpdated_Dt = DateTime.Now;
                //model.Cre_on = DateTime.Now;
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
            Instance instance = db.Instances.Find(id);
            if (instance.Instance_id == id)
            {
                instance.isActive = false;
                instance.IsDeleted = true;
                db.Entry(instance).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json("success");
        }
    }
}