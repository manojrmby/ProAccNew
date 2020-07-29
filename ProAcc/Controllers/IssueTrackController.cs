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


        public JsonResult GetTask(int Pid)
        {
            List<SelectListItem> Instance = new List<SelectListItem>();
           
                var query = from u in db.ActivityMasters where u.PhaseID == Pid && u.isActive == true orderby u.Task select u;
                
                    foreach (var v in query)
                    {
                        Instance.Add(new SelectListItem { Text = v.Task, Value = v.Activity_ID.ToString() });
                    }
            return Json(Instance, JsonRequestBehavior.AllowGet);
        } 
        public JsonResult AssignedTo(Guid Pid)
        {
            List<UserMaster> B = _Base.Sp_AssignedTo(Pid);
            return Json(B, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Getdata()
        {
            List<IssueTrackModel> ITM= _Base.Sp_GetIssueTrackData();
            

            return Json(ITM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitIssueTrack(Guid Issuetrack_Id, DateTime EndDate, String Status, String Comments)
        {
            bool Result = false;
            IssueTrackModel Data = new IssueTrackModel();
            Data.Issuetrack_Id = Issuetrack_Id;
            Data.EndDate = EndDate;            
            Data.Status = Status;
            Data.Comments = Comments;
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