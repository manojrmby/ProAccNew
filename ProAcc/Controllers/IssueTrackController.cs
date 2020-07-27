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
            ViewBag.AssignedTo = db.UserMasters.Where(x => x.isActive == true).ToList();

            return View();            
        }

        [HttpPost]
        public ActionResult Create(IssueTrackModel ism)
        {
            try
            {
                var IssueName = db.Issuetracks.Where(p => p.IssueName == ism.IssueName && p.isActive == true).ToList();
                if (IssueName.Count == 0)
                {
                    Issuetrack isstr = new Issuetrack();
                    isstr.IssueName = ism.IssueName;
                    isstr.StartDate = ism.StartDate;
                    isstr.EndDate = ism.EndDate;
                    isstr.AssignedTo = ism.AssignedTo;
                    isstr.ProjectInstance_Id = ism.ProjectInstance_Id;
                    isstr.isActive = true;
                    isstr.Cre_on = DateTime.UtcNow;
                    isstr.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.Issuetracks.Add(isstr);
                    db.SaveChanges();

                    HistoryLog HL = new HistoryLog();
                    HL.IssueTrackId = isstr.Issuetrack_Id;
                    HL.HistoryComment = ism.Comments;
                    HL.CreatedDate= DateTime.UtcNow;
                    HL.isActive = true;
                    HL.Cre_on = DateTime.UtcNow;
                    HL.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    db.HistoryLogs.Add(HL);
                    db.SaveChanges();

                    return Json("success");
                }
                else
                {
                    return Json("error");
                }

            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                //_Log.createLog(Ex, Url);
                throw;
            }

            //return View();
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