using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]

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
            List<ProjectMonitorModel> PM = _Base.Sp_GetProjectMonitorEdit(InstanceID, LoginID, PhaseId);
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

        
        public ActionResult SubmitProjectMonitor(Guid id, bool Task_Other_Environment, bool Dependency, String Pending, bool Delay_occurred, double EST_hours, double Actual_St_hours, int StatusId, DateTime Planed__St_Date, DateTime Planed__En_Date, DateTime Actual_St_Date, DateTime Actual_En_Date, String Notes)
        {
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
                Result = _Base.Sp_UpdateMonitor(Data);
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
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

        #region Assusment
        public ActionResult AssusmentMonitor()
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
            ViewBag.PhaseID= (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
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
            ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            ViewBag.PhaseID = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PreConversion && q.isActive == true select q.Id).FirstOrDefault();
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
            ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            ViewBag.PhaseID = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_PostConversion && q.isActive == true select q.Id).FirstOrDefault();
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
            ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            ViewBag.PhaseID = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Validation && q.isActive == true select q.Id).FirstOrDefault();
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
        #endregion
    }
}