using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]

    public class ProjectMonitorController : Controller
    {
        Base _Base = new Base();
        LogHelper _logHelper = new LogHelper();
        private ProAccEntities db = new ProAccEntities();
        // GET: ProjectMonitor
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult PMAdminCreate()
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
            ViewBag.Project = Project;
            return View();
        }

        public ActionResult GetData(int Phase_ID)
        {
            //int PhaseId = 5;
            int PhaseId = Phase_ID;
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            // InstanceID = Guid.Parse("52b6d7fa-18e2-4101-8040-1f8fcfd3bdaa");
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> Result = new List<ProjectMonitorModel>();
            if (Phase_ID != 1)
            {
                var query = from u in db.Instances
                            where (u.Instance_id == InstanceID && u.AssessmentUploadStatus == true)
                            select u;
                if (query.Count() > 0)
                {
                    List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitorEdit(InstanceID, LoginID, PhaseId);
                    for (int i = 0; i < PM.Count; i++)
                    {
                        if (PM[i].PhaseId == PhaseId)
                        {
                            ProjectMonitorModel projM = new ProjectMonitorModel();
                            projM = PM[i];
                            Result.Add(projM);
                        }
                    }
                }
            }
            else
            {
                List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitorEdit(InstanceID, LoginID, PhaseId);
                for (int i = 0; i < PM.Count; i++)
                {
                    if (PM[i].PhaseId == PhaseId)
                    {
                        ProjectMonitorModel projM = new ProjectMonitorModel();
                        projM = PM[i];
                        Result.Add(projM);
                    }

                }
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitProjectMonitor(int Phase_ID, Guid id, bool Task_Other_Environment, bool Dependency, String Pending, bool Delay_occurred, decimal EST_hours, decimal Actual_St_hours, int StatusId, DateTime Planed__St_Date, DateTime Planed__En_Date, DateTime Actual_St_Date, DateTime Actual_En_Date, String Notes)
        {

            int PhaseId = Phase_ID;
            ProjectMonitorModel Data = new ProjectMonitorModel();
            Data.Instance = Guid.Parse(Session["InstanceId"].ToString());
            Data.Cre_By = Guid.Parse(Session["loginid"].ToString());
            bool Result = false;
            Data.Id = id;
            Data.Task_Other_Environment = Task_Other_Environment;
            Data.Dependency = Dependency;
            Data.Pending = Pending;
            Data.Delay_occurred = Delay_occurred;
            Data.StatusId = StatusId;
            Data.EST_hours = EST_hours;
            Data.Actual_St_hours = Actual_St_hours;
            Data.Planed__St_Date = Planed__St_Date;
            Data.Planed__En_Date = Planed__En_Date;
            Data.Actual_St_Date = Actual_St_Date;
            Data.Actual_En_Date = Actual_En_Date;
            Data.Notes = Notes;

            if (Data.Id == Guid.Empty)
            {
                if (Data.Instance != Guid.Empty)
                {
                    // Result = _Base.Sp_AddNewTask(Data);
                }

            }
            else
            {
                Result = _Base.Sp_UpdateMonitor(Data, PhaseId);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #region Masters
        public ActionResult GetPhase()
        {
            List<PhaseMaster> P = _Base.GetPhaseMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetApplicationArea()
        {
            List<ApplicationAreaMaster> P = _Base.GetApplicationAreaMasters();
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

        [HttpGet]
        public ActionResult GetBlocks()
        {
            List<Buldingblock> B = new List<Buldingblock>();
            B = _Base.GetBuldingblock();
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUser(string RoleID)
        {
            List<UserMaster> P = _Base.GetUser();
            Guid InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            //  var projectId = db.Projects.Where(x => x.Instances.In == InstanceId);

            var d = (from a in db.Instances
                     join b in db.Projects
                    on a.Project_ID equals b.Project_Id
                     where a.Instance_id == InstanceId
                     select new { b.Customer_Id }).FirstOrDefault();

            List<UserMaster> Res = new List<UserMaster>();
            foreach (var item in P)
            {

                if (item.RoleID == Convert.ToInt32(RoleID))
                {
                    UserMaster UM = new UserMaster();
                    if (item.Customer_Id != null)
                    {
                        Guid CustId = d.Customer_Id;
                        if (item.Customer_Id == CustId)
                        {
                            UM = item;
                        }
                    }
                    else
                    {
                        UM = item;
                    }

                    Res.Add(UM);
                }

            }
            return Json(Res, JsonRequestBehavior.AllowGet);
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

        //public ActionResult PreviousTask()
        //{
        //    int id;
        //    if (Session["PhaseID"] != null)
        //    {
        //        id = Convert.ToInt32(Session["PhaseID"].ToString());
        //    }
        //    else
        //    {
        //        id = 1;
        //    }

        //    List<ActivityMaster> P = _Base.GetActivity(id);

        //    return Json(P, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult PreviousTaskPhase(int PhaseID)
        {
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            List<ActivityMaster> P = _Base.GetActivity(PhaseID,Instance);

            return Json(P, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //[Route("GetPhaseID/{id}")]
        //public JsonResult GetPhaseID(int id)
        //{
        //    //List<ActivityMaster> P = _Base.GetActivity(id);
        //    Session["PhaseID"] = id;
        //    return Json("success",JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetStatus()
        {
            List<StatusMaster> P = _Base.GetStatus();
            //var list = new List<int> { 1, 3, 2 };
            //string UserType = Session["UserType"].ToString();

            //if (UserType == "Admin")
            //{
            //    P.RemoveAt(2);
            //    P.RemoveAt(2);
            //    P.RemoveAt(2);
            //    //P.RemoveAt(2);
            //    //P.RemoveAt(3);
            //}
            //else if (UserType == "Consultant")
            //{
            //    P.RemoveAt(2);
            //    P.RemoveAt(3);
            //    P.RemoveAt(4);
            //}
            //else if (UserType == "Customer")
            //{

            //}
            //else if (UserType == "Project Manager")
            //{

            //}
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
        public ActionResult AddTask(ProjectMonitorModel Data)
        {
            Boolean Result = false;
            Guid Instance = Guid.Parse(Session["InstanceId"].ToString());
            Data.Cre_By = Guid.Parse(Session["loginid"].ToString());
            Result = _Base.Sp_AddTask(Instance, Data);
            if (Result == true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
           
        } 
        

        [ChildActionOnly]
        public PartialViewResult Monitor()
        {
            return PartialView("Monitor");
        }

        [ChildActionOnly]
        public PartialViewResult InstanceSelection()
        {
            ViewBag.Customer = TempData["Customer"];
            ViewBag.Instance = TempData["Instance"];
            ViewBag.Project = TempData["Project"];
            return PartialView("InstanceSelection");
        }
        //[ChildActionOnly]
        //public PartialViewResult AddTask()
        //{
        //    ViewBag.Phase = db.PhaseMasters.Where(x => x.isActive == true).OrderBy(x => x.Id);
        //    var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
        //    var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
        //    ViewBag.Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != adminRoleId && x.RoleId != pmRoleId).OrderBy(x => x.RoleName);
        //    ViewBag.Block = db.Buldingblocks.Where(x => x.isActive == true).OrderBy(x => x.Block_Name);
        //    ViewBag.ApplicationArea = db.ApplicationAreaMasters.Where(x => x.isActive == true).OrderBy(x => x.ApplicationArea);
        //    return PartialView("AddTask");
        //}

        public ActionResult GetBlock()
        {
            List<Buldingblock> B = _Base.GetBlock();
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        #region Assessment
        public ActionResult AssessmentMonitor()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
            //ViewBag.PhaseID= (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            TempData["Instance"] = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            orderby b.Project_Name
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
                            orderby b.Project_Name
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            //ViewBag.Project = Project;
            TempData["Project"] = Project;
            return View();
        }

        #endregion
        #region PreConvertion
        public ActionResult PreConvertionMonitor()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PreConversion && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            TempData["Instance"] = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            orderby b.Project_Name
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
                            orderby b.Project_Name
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            //ViewBag.Project = Project;
            TempData["Project"] = Project;
            return View();
        }
        #endregion
        #region ConvertionMonitor
        public ActionResult ConvertionMonitor()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Conversion && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            TempData["Instance"] = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            orderby b.Project_Name
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
                            orderby b.Project_Name
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            //ViewBag.Project = Project;
            TempData["Project"] = Project;
            return View();
        }

        #endregion
        #region PostConvertion
        public ActionResult PostConvertionMonitor()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PostConversion && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            TempData["Instance"] = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            orderby b.Project_Name
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
                            orderby b.Project_Name
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            TempData["Project"] = Project;
            //ViewBag.Project = Project;
            return View();
        }
        #endregion
        #region Validation
        public ActionResult ValidationMonitor()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Validation && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            TempData["Instance"] = inst;
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
            //ViewBag.Project = Project;
            TempData["Project"] = Project;
            return View();
        }
        #endregion


        #region PMTask
        public ActionResult PMTask()
        {
            
            List <SelectListItem> Project = new List<SelectListItem>();

            Guid LoginId = Guid.Parse(Session["loginid"].ToString());
            var Data = (from a in db.UserMasters
                        join b in db.Projects on a.UserId equals b.ProjectManager_Id
                        where a.UserId == LoginId && b.isActive == true
                        orderby b.Project_Name
                        select new { b.Project_Id, b.Project_Name }).ToList();
            if (Data.Count() > 0)
            {
                foreach (var v in Data)
                {
                    Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                }

            }

            ViewBag.Project = Project;
            return View();
        }

        //[HttpPost]
        public ActionResult PMTaskGetData()
        {
            string ProjectId = Session["ProjectId"].ToString();
            List<PMTaskModel> PM = _Base.GetPMTask(ProjectId);
            return Json(PM, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetprojectId(string ProjectId)
        {
            Session["ProjectId"] = ProjectId;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTaskName()
        {
            var PMTasklist = db.PMTaskMasters.Where(x => x.isActive == true).ToList();
            List<PMTaskMaster> PM = new List<PMTaskMaster>();
            foreach (var item in PMTasklist)
            {
                PMTaskMaster p = new PMTaskMaster();
                p.PMTaskId = item.PMTaskId;
                p.PMTaskName = item.PMTaskName;
                PM.Add(p);
            }
            return Json(PM, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProject()
        {
            var Projects = db.Projects.Where(x => x.isActive == true).ToList();
            List<Project> PM = new List<Project>();
            foreach (var item in Projects)
            {
                Project p = new Project();
                p.Project_Id = item.Project_Id;
                p.Project_Name = item.Project_Name;
                PM.Add(p);
            }
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitPMTaskMonitor(PMTaskMonitor_ PM)
        {
            Boolean Result = false;
            var PMTask = db.PMTaskMonitor_.Where(x => x.Id == PM.Id).FirstOrDefault();
            PMTask.Delay_occurred = PM.Delay_occurred;
            PMTask.StatusId = PM.StatusId;

            PMTask.EST_hours = PM.EST_hours;
            PMTask.Actual_St_hours = PM.Actual_St_hours;

            PMTask.Actual_St_Date = PM.Actual_St_Date;
            PMTask.Actual_En_Date = PM.Actual_En_Date;

            PMTask.Planed__St_Date = PM.Planed__St_Date;
            PMTask.Planed__En_Date = PM.Planed__En_Date;

            PMTask.Notes = PM.Notes;
            PMTask.Modified_On = DateTime.UtcNow;
            PMTask.Modified_by = Guid.Parse(Session["loginid"].ToString());
            db.Entry(PMTask).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Result = true;
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region FileUpload
        public ActionResult PMUPload()
        {
            LogHelper _log = new LogHelper();
            try
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
                //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
                TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
                TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PreConversion && q.isActive == true select q.Id).FirstOrDefault();
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
                //ViewBag.Instance = inst;
                TempData["Instance"] = inst;
                List<SelectListItem> Project = new List<SelectListItem>();

                if (User.IsInRole("Customer"))
                {
                    Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                    var Data = (from a in db.UserMasters
                                join b in db.Projects on a.Customer_Id equals b.Customer_Id
                                where a.UserId == LoginId && b.isActive == true
                                orderby b.Project_Name
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
                                orderby b.Project_Name
                                select new { b.Project_Id, b.Project_Name }).ToList();
                    if (Data.Count() > 0)
                    {
                        foreach (var v in Data)
                        {
                            Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                        }

                    }
                }
                //ViewBag.Project = Project;
                TempData["Project"] = Project;
            }
            catch (Exception ex)
            {
               
                _log.createLog(ex, "");
                
            }
            return View();
        }

        public ActionResult PM_UPload()
        {

            List<SelectListItem> Project = new List<SelectListItem>();
            Guid LoginId = Guid.Parse(Session["loginid"].ToString());
            var Data = (from a in db.UserMasters
                        join b in db.Projects on a.UserId equals b.ProjectManager_Id
                        where a.UserId == LoginId && b.isActive == true
                        orderby b.Project_Name
                        select new { b.Project_Id, b.Project_Name }).ToList();
            if (Data.Count() > 0)
            {
                foreach (var v in Data)
                {
                    Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                }

            }
            ViewBag.PDFfilepath = ConfigurationManager.AppSettings["Upload_filePath"].ToString();
            ViewBag.Project = Project;
            //TempData["Project"] = Project;

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Project Manager")]
        public ActionResult Upload()
        {
            LogHelper _log = new LogHelper();
            try
            {
                //string IDProject = Request.Params["IDProject"].ToString();
                string InstanceName = Request.Params["InstanceID"].ToString();
                string filetype = Request.Params["filetype"].ToString();
                if (InstanceName != "" && filetype != "")
                {


                    Guid Instance_ID = Guid.Parse(InstanceName);
                    Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                    if (Request.Files.Count == 1)
                    {
                        try
                        {
                            Boolean Result_Process = false;
                            HttpFileCollectionBase files = Request.Files;
                            HttpPostedFileBase file = files[0];
                            string fname;
                            string ext;
                            string NewID = Guid.NewGuid().ToString();
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                                ext = System.IO.Path.GetExtension(fname);
                            }
                            else
                            {
                                fname = file.FileName;
                                ext = System.IO.Path.GetExtension(fname);
                            }
                            String _fname = NewID + ext;
                            FileUpload _fileUpload = new FileUpload();
                            string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                            _Base.CreateIfMissing(Folder_Path);
                            fname = Path.Combine(Folder_Path, _fname);
                            file.SaveAs(fname);
                            Result_Process = _Base.PMUpload(filetype, NewID, Instance_ID, User_Id);
                            return Json("File Uploaded Successfully.");
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }

                }
                else
                {
                    return Json("Please Instance & check the files");
                }
            }
            catch(Exception ex)
            {
                _log.createLog(ex, "");
                return Json("Something Went wrong.");
            }
           
            return Json("Please check the files.");

        }


        [HttpPost]
        [Authorize(Roles = "Project Manager")]
        public ActionResult PM_Upload()
        {
            //string Cust_ID = Request.Params["Cust_ID"].ToString();
            //string IDProject = Request.Params["IDProject"].ToString();
            string InstanceName = Request.Params["InstanceID"].ToString();
            string filetype = "";
            if (Request.Files.Count >0)
            {
                //if (Request.Files.Count ==7)
                //{
                try
                {
                    bool Result_Process = false, Result_Process_Bwextractors = false,
                        Result_Process_CustomCode = false, Result_Processup_HanaDatabaseTables = false,
                        Result_Process_FioriApps = false, Result_Process_Simplification = false,
                        Result_Process_SAPReadinessCheck = false;
                    Guid Instance_ID = Guid.Parse(InstanceName);
                    Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        int FileCount = 0;
                        FileCount = Convert.ToInt32(files.AllKeys[i]) + 1;
                        HttpPostedFileBase file = files[i];
                        string fname;
                        string ext;
                        string NewID = Guid.NewGuid().ToString();
                        //Checking for Internet Explorer
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        else
                        {
                            fname = file.FileName;
                            ext = System.IO.Path.GetExtension(fname);
                        }
                        String _fname = NewID + ext;
                        FileUpload _fileUpload = new FileUpload();
                        string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                        _Base.CreateIfMissing(Folder_Path);
                        fname = Path.Combine(Folder_Path, _fname);
                        file.SaveAs(fname);
                        if (FileCount==1)//9
                        {
                            filetype = "Comprehensive Assessment & Readiness Check Report";
                        }
                        else if (FileCount == 2)//10
                        {
                            filetype = "Landscape & System Management";
                        }
                        else if (FileCount == 3)//11
                        {
                            filetype = "Custom Code Analysis";
                        }
                        else if (FileCount == 4)//12
                        {
                            filetype = "License Optimization";
                        }
                        Result_Process = _Base.PMUpload(filetype, NewID, Instance_ID, User_Id);
                        //if (FileCount == 1)
                        //{
                        //    Result_Process_Activities = _fileUpload.Process_Activities(fname, NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 2)
                        //{
                        //    Result_Process_Bwextractors = _Base.Upload_Bwextractors(NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 3)
                        //{
                        //    Result_Process_CustomCode = _fileUpload.Process_CustomCode(fname, NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 4)
                        //{
                        //    Result_Processup_HanaDatabaseTables = _Base.Upload_HanaDatabaseTables(NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 5)
                        //{
                        //    Result_Process_FioriApps = _fileUpload.Process_FioriApps(fname, NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 6)
                        //{
                        //    Result_Process_Simplification = _fileUpload.Process_Simplification(fname, NewID, Instance_ID, User_Id);
                        //}
                        //else if (FileCount == 7)
                        //{
                        //    Result_Process_SAPReadinessCheck = _Base.Upload_SAPReadinessCheck(NewID, Instance_ID, User_Id);
                        //}
                    }

                    //if (Result_Process_Bwextractors & Result_Process_Bwextractors &
                    //    Result_Process_CustomCode & Result_Processup_HanaDatabaseTables &
                    //    Result_Process_FioriApps & Result_Process_Simplification &
                    //    Result_Process_SAPReadinessCheck)
                    //{

                    //    //Result_Instance = _Base.AddInstance(IDProject, InstanceName, Instance_ID);

                    //    return Json("File Uploaded Successfully!");
                    //}
                    //Session["IsCreateAnalysisDone"] = true;
                    return Json("File Uploaded Successfully!");
                    // Returns message that successfully uploaded  

                }
                catch (Exception ex)
                {
                    _logHelper.createLog(ex);
                    String msg = ex.Message;
                    if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    else if (msg.Contains("Activities"))
                    {

                    }
                    return Json("Error occurred. Error details: " + ex.Message);
                }
                //}
                //else
                //{
                //    return Json("Please upload all Files");
                //}
            }
            else
            {
                return Json("Please choose at least one file...");
            }


        }

        public JsonResult GetFiletype(string IDInstance)
        {
            if (IDInstance != "")
            {
                Session["InstanceId"] = IDInstance;
            }
            Guid ProjectID = Guid.Empty;
            Guid IDInstanceID = Guid.Parse(IDInstance);
            Session["Instance_Name"] = db.Instances.FirstOrDefault(x => x.Instance_id == IDInstanceID).InstaceName;
            ProjectID = db.Instances.FirstOrDefault(y => y.Instance_id == IDInstanceID).Project_ID;
            Session["Project_Name"] = db.Projects.FirstOrDefault(x => x.Project_Id == ProjectID).Project_Name;


            Guid InstanceID = Guid.Parse(IDInstance);
            List<FileMaster> PM = _Base.GetPMFileList(InstanceID);
            List<SelectListItem> obj = new List<SelectListItem>();
            if (PM.Count() > 0)
            {
                foreach (var v in PM)
                {
                    obj.Add(new SelectListItem { Text = v.File, Value = v.Id.ToString() });
                }
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}