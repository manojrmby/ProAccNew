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
            var users = db.UserMasters.Where(x => x.isActive == true && x.LoginId!= "Admin").OrderByDescending(x=>x.Cre_on);
            //var users = (from e in db.UserMasters
            //            join c in db.Customers on e.Customer_Id equals c.Customer_ID where e.isActive==true && c.isActive == true
            //             select e).ToList();

            return View(users);
        }
        // GET: User
        public ActionResult CreateUsers()
        {
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType");

            var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!=1).ToList();
            ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName");

            var Cust = db.Customers.Where(x => x.isActive == true ).ToList();
            ViewBag.Customer_Id = new SelectList(Cust, "Customer_ID", "Company_Name");
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserMaster con)
        {
            if (ModelState.IsValid)
            {
                if (con.Name != null && con.LoginId != null && con.Password != null)
                {
                    con.UserId = Guid.NewGuid();
                    con.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    con.Cre_on = DateTime.Now;
                    con.isActive = true;
                    if (con.UserTypeID == 1)
                    {
                        con.RoleID = 1;
                        con.Customer_Id = null;
                    }
                    else if(con.UserTypeID == 2)
                    {
                        con.Customer_Id = null;
                    }
                    db.UserMasters.Add(con);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {

                    ViewBag.UserTypeID = new SelectList(db.User_Type, "UserTypeID", "UserType", con.UserTypeID);
                    ViewBag.Message = true;
                    return View();
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
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType",userMaster.UserTypeID);

            var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != 1).ToList();
            ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName", userMaster.RoleID);

            var Cust = db.Customers.Where(x => x.isActive == true).ToList();
            ViewBag.Customer_Id = new SelectList(Cust, "Customer_ID", "Company_Name", userMaster.Customer_Id);
            return View(userMaster);
        }

        [HttpPost]
        public ActionResult Edit(UserMaster userMaster)
        {
            if (ModelState.IsValid)
            {
                if (userMaster.Name != null)
                {
                    userMaster.Modified_On = DateTime.Now;
                    userMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    userMaster.isActive = true;
                    if (userMaster.UserTypeID == 1)
                    {
                        userMaster.RoleID = 1;
                        userMaster.Customer_Id = null;
                    }
                    else if (userMaster.UserTypeID == 2)
                    {
                        userMaster.Customer_Id = null;
                    }
                    db.Entry(userMaster).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var val1 = db.User_Type.Where(x => x.isActive == true).ToList();
                    ViewBag.UserTypeID = new SelectList(val1, "UserTypeID", "UserType", userMaster.UserTypeID);

                    var Role1 = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != 1).ToList();
                    ViewBag.RoleID = new SelectList(Role1, "RoleId", "RoleName", userMaster.RoleID);

                    var Cust1 = db.Customers.Where(x => x.isActive == true).ToList();
                    ViewBag.Customer_Id = new SelectList(Cust1, "Customer_ID", "Company_Name", userMaster.Customer_Id);
                    ViewBag.Message = true;
                    return View();
                }

            }
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType", userMaster.UserTypeID);

            var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId != 1).ToList();
            ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName", userMaster.RoleID);

            var Cust = db.Customers.Where(x => x.isActive == true).ToList();
            ViewBag.Customer_Id = new SelectList(Cust, "Customer_ID", "Company_Name", userMaster.Customer_Id);

            return View(userMaster);
            
        }

        [HttpPost]
        public ActionResult Delete(Guid id)
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

        public ActionResult ResourceAllocationCreate()
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