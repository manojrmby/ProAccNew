using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProAcc.BL.Model;
using static ProAcc.BL.Model.Common;
using System.Dynamic;
//using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]
    public class HomeController : Controller
    {
        private Guid InstanceId = Guid.Empty;
        Base _Base = new Base();
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();
        // GET: Home
        public ActionResult Home()
        {
            
             
            int Count = 1;
            int Sce = 0;
            if (Session["loginid"].ToString()!=null)
            {
                Lis Status = _Base.SpConvertionStatus(Session["InstanceId"].ToString());
                Count = Convert.ToInt32(Status.Value);
            }
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
            if (InstanceID!=Guid.Empty)
            {
                var ProId = db.Instances.Where(x => x.Instance_id == InstanceID).FirstOrDefault().Project_ID;

                Sce = db.Projects.Where(x => x.isActive == true && x.Project_Id==ProId).FirstOrDefault().ScenarioId;
                var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus==true) orderby u.InstaceName select u;
                
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
                            orderby b.Project_Name 
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count()>0)
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
           
            //var task = (from u in db.ProjectMonitors
            //            join v in db.ActivityMasters on u.ActivityID equals v.Activity_ID
            //            join w in db.StatusMasters on u.StatusId equals w.Id
            //            join x in db.UserMasters on u.UserID equals x.UserId
            //            join P in db.PhaseMasters on u.PhaseId equals P.Id
            //            where u.InstanceID == InstanceID
            //            orderby u.Modified_On descending
            //            select new { v.Task, x.Name,P.PhaseName, w.StatusName, u.Planed__En_Date }).ToList().Take(5);
            //dynamic output = new List<dynamic>();

            //foreach (var inputAttribute in task)
            //{
            //    dynamic row = new ExpandoObject();
            //    row.Task = inputAttribute.Task;
            //    row.Name = inputAttribute.Name;
            //    row.StatusName = inputAttribute.StatusName;
            //    row.Planed__En_Date = inputAttribute.Planed__En_Date;
            //    row.PhaseName = inputAttribute.PhaseName;
            //    output.Add(row);
            //}
            ViewBag.count = Count;
            //ViewBag.Taskdetails = output;
            ViewBag.Project = Project;
            ViewBag.Scen= Sce;

            //Mail m = new Mail();
            //m.SendEmail();
            return View();
        }

        //public ActionResult Home1()
        //{
        //    return View();
        //}
        [HttpPost]
        public JsonResult GetHomeDonut()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());

            List<SP_HomeDonut_Result> GetRelevant = _Base.SAP_Home_Donut(InstanceId); 
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }




        public ActionResult Test()
        {
            //List<Customer> cust = db.Customers.Where(a => a.isActive == true).ToList();
            //ViewBag.list = cust;
            return View();
        }
        public JsonResult SubmitInstance(string IDInstance)
        {
            if (IDInstance != "")
            {
                //_Base.AddInstance(IDInstance);
                Session["InstanceId"] = IDInstance;
            }
            Guid ProjectID = Guid.Empty;
            Guid IDInstanceID = Guid.Parse(IDInstance);
            Session["Instance_ID"] = IDInstanceID;
            Session["Instance_Name"] = db.Instances.FirstOrDefault(x => x.Instance_id == IDInstanceID).InstaceName;
            ProjectID = db.Instances.FirstOrDefault(y => y.Instance_id == IDInstanceID).Project_ID;
            Session["Project_ID"] = ProjectID;
            Session["Project_Name"] = db.Projects.FirstOrDefault(x => x.Project_Id == ProjectID).Project_Name;
            //var d = db.FileUploadMasters.FirstOrDefault(x => x.InstanceID == IDInstanceID).Id;
            bool Res = false;
            var data = db.FileUploadMasters.Count(x => x.InstanceID == IDInstanceID);
            if (data!=0)
            {
                Res = true;
            }
            Session["IsCreateAnalysisDone"] = Res;
            return Json(IDInstance, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadInstance(string ProjectId)
        {   
            //GeneralList Instance = _Base.GetInstanceDropdown(ProjectId);
            List<SelectListItem> Instance = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(ProjectId)&& ProjectId !="0")
            {
                var ID = Guid.Parse(ProjectId);
                var query = from u in db.Instances where u.Project_ID == ID && u.isActive == true orderby u.InstaceName select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Instance.Add(new SelectListItem { Text = v.InstaceName, Value = v.Instance_id.ToString() });
                    }
                }
            }
            return Json(Instance, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadProject(string CustomerId)
        {
            List<SelectListItem> Project = new List<SelectListItem>();
            //if (User.IsInRole("Admin"))
            //{
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    Guid IDCustomer = Guid.Parse(CustomerId);
                    var query = from u in db.Projects where (u.Customer_Id == IDCustomer && u.isActive == true) orderby u.Project_Name select u;
                    if (query.Count() > 0)
                    {
                        foreach (var v in query)
                        {
                            Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                        }
                    }
                }
            //}
            //else if (User.IsInRole("Consultant"))
            //{
            //    Guid loginId = Guid.Parse(Session["loginid"].ToString());
            //    if (!string.IsNullOrEmpty(CustomerId))
            //    {
            //        Guid IDCustomer = Guid.Parse(CustomerId);
            //        var query = from u in db.Instances where (u.CustomerID == IDCustomer && u.ConsultantID== loginId && u.isActive == true ) select u;
            //        if (query.Count() > 0)
            //        {
            //            foreach (var v in query)
            //            {
            //                Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
            //            }
            //        }
            //    }

            //}
         
            
            
            return Json(Project, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LoadInstanceforResource(string ProjectId)
        {
            //GeneralList Instance = _Base.GetInstanceDropdown(ProjectId);
            List<SelectListItem> Instance = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(ProjectId) && ProjectId != "0")
            {
                var ID = Guid.Parse(ProjectId);
                var query = from u in db.Instances where u.Project_ID == ID && u.isActive == true orderby u.InstaceName select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Instance.Add(new SelectListItem { Text = v.InstaceName, Value = v.Instance_id.ToString() });
                    }
                }
            }
            return Json(Instance, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult LoadInstanceforResource(string ProjectId, int PhaseID)
        //{
        //    //GeneralList Instance = _Base.GetInstanceDropdown(ProjectId);
        //    List<SelectListItem> Instance = new List<SelectListItem>();


        //    if (!String.IsNullOrEmpty(ProjectId) && ProjectId != "0")
        //    {
        //        var ID = Guid.Parse(ProjectId);
        //        var query = from u in db.Instances where u.Project_ID == ID select u;
        //        if (query.Count() > 0)
        //        {
        //            foreach (var v in query)
        //            {
        //                Instance.Add(new SelectListItem { Text = v.InstaceName, Value = v.Instance_id.ToString() });
        //            }
        //        }
        //    }
        //    return Json(Instance, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult UpdatePassword()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult UpdatePassword(UserMaster userMaster)
        {
            try
            {
                if (userMaster.Password != null)
                {

                    userMaster.Modified_On = DateTime.UtcNow;
                    userMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    userMaster.isActive = true;
                    userMaster.Password = _Base.Encrypt(userMaster.Password.ToString());
                    userMaster.UserId = Guid.Parse(Session["loginid"].ToString());
                    bool Result = _Base.Sp_ResetPassword(userMaster);

                    //bool Status = true;
                    //Guid CreatedBy= Guid.Parse(Session["loginid"].ToString());
                    //DateTime CreatedOn = DateTime.UtcNow;
                    //bool IsValid = true;
                    //Guid UserId = Guid.Parse(Session["loginid"].ToString()); 

                    //bool Reset = _Base.Sp_ResetPasswordStatus(UserId,Status,CreatedBy, CreatedOn, IsValid);

                    if (Result == true)
                    {
                        //Session.Clear();
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("fail", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.UserTypeID = db.User_Type.Where(x => x.isActive == true).ToList();
                    var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
                    var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
                    ViewBag.RoleID = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != adminRoleId && x.RoleId != pmRoleId).ToList();
                    ViewBag.Customer_Id = db.Customers.Where(x => x.isActive == true).ToList();

                    ViewBag.Message = true;
                    return View();
                }
            }
            catch (Exception ex)
            {
                string Url = Request.Url.AbsoluteUri;
                //_Log.createLog(ex, "-->Users Edit Post" + Url);
                throw;
            }
        }


        public ActionResult OldPasswordcheck(string password)
        {
            string passwordEncrypt = _Base.Encrypt(password);
            var result = db.UserMasters.ToList().Exists(x => x.Password == passwordEncrypt && x.isActive == true);
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }
        }

    }
}