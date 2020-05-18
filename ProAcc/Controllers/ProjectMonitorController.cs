using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant")]

    public class ProjectMonitorController : Controller
    {
        Base _Base = new Base();
        private ProAccEntities db = new ProAccEntities();
        // GET: ProjectMonitor
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult PMAdminCreate()
        {
            int j = 0;
            var stat = db.HanaStatus.ToList();
            for (int i = 0; i < stat.Count(); i++)
            {
                if (stat[i].IsActive == true)
                {
                    j = j + 1;
                }
            }
            ViewBag.count = j;

            int userType = 0;
            if (User.IsInRole("Admin"))
            {
                userType = 1;
            }
            else if (User.IsInRole("Consultant"))
            {
                userType = 2;
            }
            else if (User.IsInRole("Customer"))
            {
                userType = 3;
            }
            GeneralList sP_ = _Base.spCustomerDropdown(Session["loginid"].ToString(), userType);
            ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            int inst = 0;
            if (InstanceID != Guid.Empty)
            {
                var q = from u in db.ProjectInstanceConfigs where (u.Id == InstanceID && u.UploadStatus == true) select u;
                if (q.Count() > 0)
                {
                    inst = 1;
                }
                else { inst = 0; }

            }
            ViewBag.Instance = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid customerId = Guid.Parse(Session["loginid"].ToString());
                var query = from u in db.CustomerProjectConfigs where (u.CustomerID == customerId && u.isActive == true) select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                    }
                }
            }
            else
            {

                //var query = from u in db.CustomerProjectConfigs where(u.isActive==true)  select u;
                //if (query.Count() > 0)
                //{
                //    foreach (var v in query)
                //    {
                //        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                //    }
                //}
            }


            ViewBag.Project = Project;
            return View();
        }
        public ActionResult GetData()
        {
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID);
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitProjectMonitor(ProjectMonitorModel Data)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
        #region Masters
        public ActionResult GetPhase()
        {
            List<PhaseMaster> P = _Base.GetPhaseMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPending()
        {
            List<PendingMaster> P = _Base.GetPendingMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTeam()
        {
            List<TeamMaster> P = _Base.GetTeamMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConsultant()
        {
            List<Consultant> P = _Base.GetConsultant();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStatus()
        {
            List<StatusMaster> P = _Base.GetStatus();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllInstance()
        {
            //GeneralList Instance = _Base.GetInstanceDropdown(ProjectId);
            List<SelectListItem> Instance = new List<SelectListItem>();
           
                
                var query = from u in db.ProjectInstanceConfigs select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Instance.Add(new SelectListItem { Text = v.InstaceName, Value = v.Id.ToString() });
                    }
                }
            
            return Json(Instance, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}