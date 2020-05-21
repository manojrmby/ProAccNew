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
                var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus== true) select u;
                if (q.Count() > 0)
                {
                    inst = 1;
                }
                else { inst = 0; }

            }
            ViewBag.Instance = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            //if (User.IsInRole("Customer"))
            //{
            //    Guid customerId = Guid.Parse(Session["loginid"].ToString());
            //    var query = from u in db.cus where (u.CustomerID == customerId && u.isActive == true) select u;
            //    if (query.Count() > 0)
            //    {
            //        foreach (var v in query)
            //        {
            //            Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
            //        }
            //    }
            //}
            //else
            //{

            var query = from u in db.Projects where (u.isActive == true) select u;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                }
            }
            // }


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
            string InstanceId = Session["InstanceId"].ToString();
            Data.Cre_By = Guid.Parse(Session["loginid"].ToString());
            //bool s=_Base.Sp_AdminAddMonitor(Data, InstanceId);
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #region Masters
        public ActionResult GetPhase()
        {
            List<PhaseMaster> P = _Base.GetPhaseMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetPending()
        //{
        //    List<PendingMaster> P = _Base.GetPendingMasters();
        //    return Json(P, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetRole()
        {
            List<RoleMaster> P = _Base.GetRoleMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUser()
        {
            List<UserMaster> P = _Base.GetUser();
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
           
                
                var query = from u in db.Instances select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Instance.Add(new SelectListItem { Text = v.InstaceName, Value = v.Instance_id.ToString() });
                    }
                }
            
            return Json(Instance, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}