using ProAcc.BL;
using ProAcc.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProAcc.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        // GET: Login

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Validate(UserModel user, string returnUrl)
        {
            try
            {
             LogedUser logedUser = new LogedUser();
            logedUser.Username = user.Username;
            logedUser.Password = user.Password;
            string decodedUrl = "";
            if (!string.IsNullOrEmpty(returnUrl))
                decodedUrl = Server.UrlDecode(returnUrl);
           
                if (!String.IsNullOrEmpty(user.Username)&& (!String.IsNullOrEmpty(user.Password)))
                {
                    logedUser = _Base.UserValidation(logedUser);
                    if (logedUser.ID != Guid.Empty)
                    {
                        FormsAuthentication.SetAuthCookie(logedUser.Username, false);
                        Session["loginid"] = logedUser.ID.ToString();
                        Session["UserName"] = logedUser.Name.ToString();
                        Session["InstanceId"] = Guid.Empty;
                        string UserType = "";
                        if (logedUser.Type == 1)
                        {
                            UserType = "Admin";
                        }
                        else if (logedUser.Type == 2)
                        {
                            UserType = "Consultant";
                        }
                        else if (logedUser.Type == 3)
                        {
                            UserType = "Customer";
                        }
                        Session["UserType"] = UserType;
                        
                       // return RedirectToAction("Home", "Home")
                       return RedirectToAction("Create", "Customers");
                    }
                    else
                    {
                        TempData["Message"] = "User name & password supplied doesn't Match";
                    }
                }
                else
                {
                    TempData["Message"] = "Enter User name & password";
                }
            }
            catch (Exception ex)
            {

                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(ex, Url);
                TempData["Message"] = "Login failed.Error - " + ex.Message;
                //throw;
            }
            return RedirectToAction("Login", "Login");
        }


        //public JsonResult ValidateUser(LogedUser user)
        //{

        //    string IsValidate = string.Empty;
        //    user = _Base.UserValidation(user);
        //    if (user.ID != Guid.Empty)
        //    {
        //        IsValidate = "Login Successfully.";
        //    }

        //    //switch (userId)
        //    //{
        //    //    case -1:
        //    //        IsValidate = "Username and/or password is incorrect.";
        //    //        break;
        //    //    case -2:
        //    //        IsValidate = "Account has not been activated.";
        //    //        break;
        //    //    case -3:
        //    //        IsValidate = "Login Successfully.";
        //    //        break;
        //    //}
        //    return Json(IsValidate, JsonRequestBehavior.AllowGet);
        //}

    }

}

