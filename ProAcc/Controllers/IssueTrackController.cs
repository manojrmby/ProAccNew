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
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]
    public class IssueTrackController : Controller
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        private ProAccEntities db = new ProAccEntities();
        // GET: IssueTrack
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexPage()
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
            //TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            //TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
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
            ViewBag.Project = Project;
            //TempData["Project"] = Project;

            ////int userType = 0;
            //List<IssueTrackModel> ITM = new List<IssueTrackModel>();
            //if (User.IsInRole("Consultant"))
            //{
            //    userType = 2;
            //}
            //else if (User.IsInRole("Customer"))
            //{
            //    userType = 3;
            //}
            //else if (User.IsInRole("Project Manager"))
            //{
            //    userType = 4;
            //}

            //ITM = _Base.Sp_GetIssueTrackData(Session["loginid"].ToString(), userType);
            //ViewBag.ITM = ITM;
            return View();
        }

        [HttpGet]
        public ActionResult IssueTrackDatas(IssueTrackModel model)
        {
            int userType = 0;
            List<IssueTrackModel> ITM = new List<IssueTrackModel>();
            if (User.IsInRole("Consultant"))
            {
                userType = 2;
            }
            else if (User.IsInRole("Customer"))
            {
                userType = 3;
            }
            else if (User.IsInRole("Project Manager"))
            {
                userType = 4;
            }

            ITM = _Base.Sp_GetIssueTrackData(Session["loginid"].ToString(), userType,model);
            //ViewBag.ITM = ITM;

            //var obj = new { data = ITM };
            return PartialView("_IssueTrackIndex", ITM);
        }


        public ActionResult Create()
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
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");           
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
            //ViewBag.AssignedTo = db.UserMasters.Where(x => x.isActive == true).ToList();
            ViewBag.PhaseMaster = new SelectList(db.PhaseMasters, "Id", "PhaseName");
            return View();            
        }

        [HttpPost]
        public ActionResult Create(IssueTrackModel ism)
        {
            bool Result = false;

            ism.Cre_By = Guid.Parse(Session["loginid"].ToString());
            ism.StartDate= DateTime.UtcNow;
            ism.EndDate= DateTime.UtcNow;
            
            Result = _Base.Sp_InsertIssue(ism);
            if(Result==true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }           
        }


        
        public ActionResult GetIssueTrackById(Guid id)
        {
            try
            {
                IssueTrackModel ITM = _Base.Sp_EditIssueTrackData(id);
                // ViewBag.ITM = ITM;
                //List<IssueTrackModel> ITM_Comments = _Base.Sp_GetIssueTrackData();
                ViewBag.ITM_Comments = db.HistoryLogs.Where(x=>x.IssueTrackId==id && x.isActive==true).ToList();
                List<UserMaster> B = _Base.Sp_EditAssignedTo(ITM.ProjectInstance_Id);
                ViewBag.AssignedTo = B;
                
                var loginid = Guid.Parse(Session["loginid"].ToString());
                var a = (from i in db.Issuetracks
                         join p in db.Projects on i.Cre_By equals p.ProjectManager_Id
                         where i.Cre_By == loginid
                         select i).FirstOrDefault();
                ViewBag.status = false;
                if (ITM.Cre_By.ToString() == Session["loginid"].ToString())
                {
                    if (a == null)
                    {
                        ViewBag.status = true;
                    }
                }                   
                
                return View(ITM);
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, "-->GetIssueTrackById" + Url);
                throw;
            }
        }

        public JsonResult EditAssignedTo(Guid Iid)
        {
            List<UserMaster> B = _Base.Sp_EditAssignedTo(Iid);
            return Json(B, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTask(int Pid,Guid insID)
        {
            List<SelectListItem> Instance = new List<SelectListItem>();

            //var query = from u in db.ActivityMasters where u.PhaseID == Pid && u.isActive == true orderby u.Task select u;

            //foreach (var v in query)
            //{
            //    Instance.Add(new SelectListItem { Text = v.Task, Value = v.Activity_ID.ToString() });
            //}
            var query = from p in db.ProjectMonitors
                        join a in db.ActivityMasters on p.ActivityID equals a.Activity_ID
                        where p.isActive == true && p.InstanceID==insID && p.PhaseId==Pid
                        orderby a.Task
                        select new { p.ActivityID, a.Task };
                //db.ProjectMonitors.Where(x => x.InstanceID == insID && x.PhaseId == Pid && x.isActive == true).ToList();
            //from u in db.ActivityMasters where u.PhaseID == Pid && u.isActive == true orderby u.Task select u;

            foreach (var v in query)
            {
                Instance.Add(new SelectListItem { Text = v.Task, Value = v.ActivityID.ToString() });
            }

            return Json(Instance, JsonRequestBehavior.AllowGet);
        } 
        public JsonResult AssignedTo(Guid Pid)
        {
            List<UserMaster> B = _Base.Sp_AssignedTo(Pid);
            return Json(B, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult Getdata()
        //{
        //    List<IssueTrackModel> ITM= _Base.Sp_GetIssueTrackData(Session["loginid"].ToString());
            

        //    return Json(ITM, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SubmitIssueTrack(Guid Issuetrack_Id,  String Status, Guid AssignedTo,String Comments,String Description) //DateTime EndDate,
        {
            bool Result = false;
            IssueTrackModel Data = new IssueTrackModel();
            Data.Issuetrack_Id = Issuetrack_Id;
            Data.EndDate = DateTime.UtcNow; //EndDate;            
            Data.Status = Status;
            Data.Comments = Comments;
            Data.AssignedTo = AssignedTo;
            Data.Description = Description;
            Data.Modified_by= Guid.Parse(Session["loginid"].ToString());
           
            Result = _Base.Sp_UpdateIssueTrack(Data);
          
            if(Result==true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
            
        }
        public ActionResult GetInstance()
        {
            List<Instance> P = _Base.GetInstanceMasters();
            return Json(P, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public PartialViewResult InstanceSelection()
        {
            ViewBag.Customer = TempData["Customer"];
            ViewBag.Instance = TempData["Instance"];
            ViewBag.Project = TempData["Project"];
            return PartialView("InstanceSelection");
        }
    }
}