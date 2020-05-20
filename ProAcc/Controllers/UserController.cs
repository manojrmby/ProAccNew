using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();

        public ActionResult Index()
        {
            var users = db.UserMasters.Where(x => x.isActive == true);
            return View(users);
        }
        // GET: User
        public ActionResult CreateUsers()
        {
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.UserTypeID = new SelectList(val, "UserTypeID", "UserType");

            var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!=1).ToList();
            ViewBag.RoleID = new SelectList(Role, "RoleId", "RoleName"); //Role_Data

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
                if (userMaster.Name != null && userMaster.LoginId != null)
                {
                    userMaster.Modified_On = DateTime.Now;
                    userMaster.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    userMaster.isActive = true;
                    if (userMaster.UserTypeID == 1)
                    {
                        userMaster.RoleID = 1;
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
    }
}