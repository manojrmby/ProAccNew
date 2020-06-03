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
        public ActionResult Validate(UserModel user, string ReturnUrl)
        {
            try
            {
             LogedUser logedUser = new LogedUser();
            logedUser.Username = user.Username;
            logedUser.Password = user.Password;
                
               if (!String.IsNullOrEmpty(user.Username)&& (!String.IsNullOrEmpty(user.Password)))
                {
                    logedUser = _Base.UserValidation(logedUser);
                    if (logedUser.ID != Guid.Empty)
                    {
                        FormsAuthentication.SetAuthCookie(logedUser.Username, false);
                        Session["loginid"] = logedUser.ID.ToString();
                        Session["UserName"] = logedUser.Name.ToString();
                        Session["InstanceId"] = Guid.Empty;
                        //Session["UserTypeID"] = logedUser.Type;
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
                        else if (logedUser.Type == 4)
                        {
                            UserType = "Project Manager";
                        }
                        Session["UserType"] = UserType;
                        if (!string.IsNullOrEmpty(Request.Form["ReturnUrl"]))
                        {
                            String Controller = Request.Form["ReturnUrl"].Split('/')[1].ToString();
                            String Action = Request.Form["ReturnUrl"].Split('/')[2].ToString();

                            return RedirectToAction(Action,Controller);
                        }
                        else
                        {
                            return RedirectToAction("Home", "Home");
                        }
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
    }

}

