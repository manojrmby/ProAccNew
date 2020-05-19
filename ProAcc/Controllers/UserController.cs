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
    }
}