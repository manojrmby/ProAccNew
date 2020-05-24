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
    public class ResourceController : Controller
    {
        Base _Base = new Base();
        private ProAccEntities db = new ProAccEntities();
        // GET: Resource
        public ActionResult ResourceAssessment()
        {
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
                var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus == true) select u;
                if (q.Count() > 0)
                {
                    inst = 1;
                }
                else { inst = 0; }

            }
            ViewBag.Instance = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            var query = from u in db.Projects where (u.isActive == true) select u;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                }
            }
            ViewBag.Project = Project;
            return View();
        }


        public ActionResult GetData()
        {
            int PhaseId = 5;
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID);
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            for (int i = 0; i < PM.Count; i++)
            {
                if (PM[i].PhaseId == PhaseId)
                {
                    ProjectMonitorModel projM = new ProjectMonitorModel();
                    projM = PM[i];
                    Result.Add(projM);
                }
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateResource(ProjectMonitorModel Data)
        {
            Data.Instance = Guid.Parse(Session["InstanceId"].ToString());
            Data.Modified_by = Guid.Parse(Session["loginid"].ToString());
            Data.Modified_On = DateTime.UtcNow;
            if (Data.Id!=Guid.Empty)
            {
                bool s = _Base.Sp_UpdateResourceTask(Data);
            }
            
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteResource(String ID)
        {
            ProjectMonitorModel PM = new ProjectMonitorModel();
            PM.Id = Guid.Parse(ID);
            PM.Modified_by = Guid.Parse(Session["loginid"].ToString());
            PM.Modified_On = DateTime.UtcNow;
            bool s = _Base.Sp_DeleteResourceTask(PM);

            return Json(s, JsonRequestBehavior.AllowGet);
        }
            public ActionResult GetAllUser()
        {
            List<UserMaster> P = _Base.GetUser();
            //List<UserMaster> Res = new List<UserMaster>();
            //foreach (var item in P)
            //{

               
            //        UserMaster UM = new UserMaster();
            //        UM = item;
            //        Res.Add(UM);
            //    }

            //}
            return Json(P, JsonRequestBehavior.AllowGet);
        }
    }
}