using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        Base _Base = new Base();
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();

        public ActionResult Index()
        {
            List<UserMaster> u = new List<UserMaster>();
            var users = db.UserMasters.Where(x => x.LoginId!= "Admin").OrderByDescending(x=>x.Cre_on).ToList();
            users.ForEach(p => {
                if (p.UserTypeID == 3 )
                {
                    if (p.Customer_Id != null)
                    {
                        p.Customer = db.Customers.Where(pp => pp.Customer_ID == p.Customer_Id).FirstOrDefault();
                        if (p.Customer.IsDeleted == false)
                        {
                            u.Add(p);
                        }
                    }
                }
                else
                {
                    u.Add(p);
                }
            });
           
            return View(u);
        }
        // GET: User
        public ActionResult CreateUsers()
        {
            //var val = db.User_Type.Where(x => x.isActive == true).ToList();
            //ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType");

            //var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!=1 && x.RoleId!=10).ToList();
            //ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName");

            //var Cust = db.Customers.Where(x => x.isActive == true ).ToList();
            //ViewBag.Customer_Id = new SelectList(Cust, "Customer_ID", "Company_Name");
            //return View();

            ViewBag.UserTypeID = db.User_Type.Where(x => x.isActive == true).ToList();
            var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
            var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
            ViewBag.RoleID = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != adminRoleId && x.RoleId != pmRoleId).ToList();
            ViewBag.Customer_Id = db.Customers.Where(x => x.isActive == true).ToList();

            return View();
        }

        public JsonResult NameAvailability(string namedata,Guid? id)
        {
            if(id!=null)
            {
                var SearchDt = db.UserMasters.Where(x => x.Name == namedata).Where(x => x.UserId!=id && x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = db.UserMasters.Where(x => x.Name == namedata).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            
        }
        public JsonResult UsernameAvailability(string userdata,Guid? id)
        {
            if(id!=null)
            {
                var SearchDt = db.UserMasters.Where(x => x.LoginId == userdata).Where(x =>x.UserId!=id && x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = db.UserMasters.Where(x => x.LoginId == userdata).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }

           
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(UserMaster con)
        {
            if (ModelState.IsValid)
            {
                if (con.Name != null && con.LoginId != null && con.Password != null)
                {
                    con.UserId = Guid.NewGuid();
                    con.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    con.Cre_on = DateTime.UtcNow;
                    con.isActive = true;
                    //var usertypeid = db.User_Type.Where(x => x.UserTypeID==con.UserTypeID && x.isActive == true).FirstOrDefault().UserTypeID;
                    if (con.UserTypeID == 1)
                    {
                        var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
                        con.RoleID = adminRoleId;
                        con.Customer_Id = null;
                    }
                    else if(con.UserTypeID == 2)
                    {
                        con.Customer_Id = null;
                    }
                    else if (con.UserTypeID == 4)
                    {
                        var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
                        con.RoleID = pmRoleId;
                        con.Customer_Id = null;
                    }
                    db.UserMasters.Add(con);
                    db.SaveChanges();
                    return Json("success");
                    //return RedirectToAction("Index");
                }
                else
                {
                    return Json("error");
                    //ViewBag.UserTypeID = new SelectList(db.User_Type, "UserTypeID", "UserType", con.UserTypeID);
                    //ViewBag.Message = true;
                    //return View();
                }

            }

            ViewBag.UserTypeID = new SelectList(db.User_Type, "UserTypeID", "UserType", con.UserTypeID);
            return View(con);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMaster userMaster = db.UserMasters.Find(id);
            if (userMaster == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserTypeID = db.User_Type.Where(x => x.isActive == true).ToList();
            var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
            var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
            ViewBag.RoleID = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != adminRoleId && x.RoleId != pmRoleId).ToList();
            ViewBag.Customer_Id = db.Customers.Where(x => x.isActive == true).ToList();

            //var val = db.User_Type.Where(x => x.isActive == true).ToList();
            //ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType",userMaster.UserTypeID);

            //var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != 1).ToList();
            //ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName", userMaster.RoleID);

            //var Cust = db.Customers.Where(x => x.isActive == true).ToList();
            //ViewBag.Customer_Id = new SelectList(Cust, "Customer_ID", "Company_Name", userMaster.Customer_Id);
            return View(userMaster);
        }

        [HttpPost]
        public ActionResult Edit(UserMaster userMaster)
        {
            
            if (userMaster.Name != null)
            {
                 userMaster.Modified_On = DateTime.UtcNow;
                 userMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                 userMaster.isActive = true;
                if (userMaster.UserTypeID == 1)
                {
                    var adminRoleId = db.RoleMasters.Where(x => x.RoleName == "Admin" && x.isActive == true).FirstOrDefault().RoleId;
                    userMaster.RoleID = adminRoleId;
                    userMaster.Customer_Id = null;
                }
                else if (userMaster.UserTypeID == 2)
                {
                    userMaster.Customer_Id = null;
                }
                else if (userMaster.UserTypeID == 4)
                {
                    var pmRoleId = db.RoleMasters.Where(x => x.RoleName == "Project Manager" && x.isActive == true).FirstOrDefault().RoleId;
                    userMaster.RoleID = pmRoleId;
                    userMaster.Customer_Id = null;
                }
                db.Entry(userMaster).State = EntityState.Modified;
                 db.SaveChanges();
                 return Json("success");
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

        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            var del = (from a in db.UserMasters
                       join b in db.ProjectMonitors
                       on a.UserId equals b.UserID
                       where a.UserId == id && (b.StatusId!=1&& b.StatusId != 3) && b.isActive == true 
                       select b).ToList();
            var delpm = (from a in db.UserMasters
                         join b in db.Projects
                         on a.UserId equals b.ProjectManager_Id
                         where a.UserId == id && a.isActive == true && b.isActive == true
                         select b).ToList();
            if(del.Count!=0 || delpm.Count!=0)            
            {
                //var projectname = (from a in db.Projects
                //                   join b in db.Instances on a.Project_Id equals b.Project_ID
                //                   join c in db.ProjectMonitors on b.Instance_id equals c.InstanceID
                //                   where c.UserID == id select a.Project_Name).ToList();
                return Json("fail");
            }
            else
            {
                UserMaster user = db.UserMasters.Find(id);
                if (user.UserId == id)
                {
                    user.isActive = false;
                    user.IsDeleted = true;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json("success");
            }
        }

        public ActionResult ResourceAllocationCreate()
        {
            int j = 0;
            //var stat = db.HanaStatus.ToList();
            //for (int i = 0; i < stat.Count(); i++)
            //{
            //    if (stat[i].IsActive == true)
            //    {
            //        j = j + 1;
            //    }
            //}
            //ViewBag.count = j;

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
        public ActionResult GetResourceAllocationData(String ProjectID)
        {
            //InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            //InstanceId = Guid.Parse("d9747ed5-f482-4ac2-9ef9-4a25aa5a6855");
            var Result = _Base.SP_GetResourceAllocation(ProjectID);
           // List<ResourceAllocation> CR = db.ResourceAllocations.Where(x => x.isActive == true).ToList();


            var obj = new { data = Result };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitResource(String IDS, String PtojectId)
        {
            bool Result = false;
            Result = _Base.SP_SubmitResource(IDS, Guid.Parse(PtojectId), Guid.Parse(Session["loginid"].ToString()));

            return Json(Result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadUser()
        {
            var Data = _Base.SP_GetAllUser();
            List<SelectListItem> User = new List<SelectListItem>();



            //if (!String.IsNullOrEmpty(ProjectId) && ProjectId != "0")
            //{
            //    var ID = Guid.Parse(ProjectId);
            //    var query = from u in db.UserMasters where u.isActive == true && u.IsDeleted == false select u;
            //    if (query.Count() > 0)
            //    {
            //        foreach (var v in query)
            //        {
            //            User.Add(new SelectListItem { Text = v.Name, Value = v.UserId.ToString() });
            //        }
            //    }
            //}
            return Json(Data, JsonRequestBehavior.AllowGet);
        }
    }
}