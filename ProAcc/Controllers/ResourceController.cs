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
    [Authorize(Roles = "Admin,Consultant,Project Manager")]
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
                var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus == true && u.isActive == true) select u;
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
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            else if (User.IsInRole("Project Manager"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.UserId equals b.ProjectManager_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }



            ViewBag.Project = Project;
            return View();
        }

        public ActionResult GetData()
        {
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID, PhaseId);
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

        public ActionResult MasterAddAssessment()
        {
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            //Result = db.ActivityMastersWhere(x => x.isActive == true).OrderBy(a => a.Sequence_Num)
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
            Result = _Base.Sp_GetMasterAdd(Instance, PhaseId);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddResource(ProjectMonitorModel Data)
        {
            Boolean Result = false;
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            Result = _Base.Sp_AddResource(Instance,Data.ActivityID, Session["loginid"].ToString());
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourcePreConversion()
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

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            else if (User.IsInRole("Project Manager"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.UserId equals b.ProjectManager_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }

            ViewBag.Project = Project;
            return View();
        }

        public ActionResult GetDataPreConversion()
        {
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PreConversion && q.isActive == true select q.Id).FirstOrDefault();
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID, PhaseId);
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

        public ActionResult MasterAddPreConversion()
        {
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            //Result = db.ActivityMastersWhere(x => x.isActive == true).OrderBy(a => a.Sequence_Num)
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PreConversion && q.isActive == true select q.Id).FirstOrDefault();
            Result = _Base.Sp_GetMasterAdd(Instance, PhaseId);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ResourceMigration()
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

        public ActionResult GetDataMigration()
        {
            int PhaseId = 4;
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID, PhaseId);
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
        public ActionResult ResourcePostConversion()
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

        public ActionResult GetDataPostConversion()
        {
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PostConversion && q.isActive == true select q.Id).FirstOrDefault();
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID, PhaseId);
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

        public ActionResult MasterAddPostConversion()
        {
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            //Result = db.ActivityMastersWhere(x => x.isActive == true).OrderBy(a => a.Sequence_Num)
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PostConversion && q.isActive == true select q.Id).FirstOrDefault();
            Result = _Base.Sp_GetMasterAdd(Instance, PhaseId);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ResourceValidation()
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

        public ActionResult GetDataValidation()
        {
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Validation && q.isActive == true select q.Id).FirstOrDefault();
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitor(InstanceID, LoginID, PhaseId);
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

        public ActionResult MasterAddValidation()
        {
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            //Result = db.ActivityMastersWhere(x => x.isActive == true).OrderBy(a => a.Sequence_Num)
            int PhaseId = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Validation && q.isActive == true select q.Id).FirstOrDefault();
            Result = _Base.Sp_GetMasterAdd(Instance, PhaseId);

            return Json(Result, JsonRequestBehavior.AllowGet);
        }
    }
}