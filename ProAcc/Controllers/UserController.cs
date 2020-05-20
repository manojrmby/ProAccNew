using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
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
        // GET: User
        public ActionResult CreateUsers()
        {
            var val = db.User_Type.Where(x => x.isActive == true).ToList();
            ViewBag.Type = new SelectList(val, "UserTypeID", "UserType");

            var Role = db.RoleMasters.Where(x => x.isActive == true && x.RoleId!=1).ToList();
            ViewBag.Role_Data = new SelectList(Role, "RoleId", "RoleName");

            var Cust = db.Customers.Where(x => x.isActive == true ).ToList();
            ViewBag.Cust = new SelectList(Cust, "Customer_ID", "Company_Name");
            return View();
        }

        public JsonResult NameAvailability(string namedata)
        {
            System.Threading.Thread.Sleep(100);
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
        public JsonResult UsernameAvailability(string userdata)
        {
            System.Threading.Thread.Sleep(100);
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

                    ViewBag.UserTypeID = new SelectList(db.User_Type, "Id", "UserType", con.UserTypeID);
                    ViewBag.Message = true;
                    return View();
                }

            }

            ViewBag.UserTypeID = new SelectList(db.User_Type, "Id", "UserType", con.UserTypeID);
            return View(con);
        }
    }
}