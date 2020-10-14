using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using static ProAcc.BL.Base;

namespace ProAcc.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        Base _Base = new Base();
        
        private ProAccEntities db = new ProAccEntities();
        RstPassword rt = new RstPassword();
        // GET: Login

        public ActionResult Login()
        {
            if (Session["loginid"] !=null)
            {
                return RedirectToAction("Home", "Home");
            }
            else
            {
                return View();
            }
                
        }
       
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Validate(UserModel user, string ReturnUrl)
        {
            LogHelper _Log = new LogHelper();
            try
            {
                
             LogedUser logedUser = new LogedUser();
            logedUser.Username = user.Username;
            logedUser.Password = user.Password;
                _Log.createLog("Start ---> Validadate");
                if (!String.IsNullOrEmpty(user.Username)&& (!String.IsNullOrEmpty(user.Password)))
                {
                    //Boolean a = Convert.ToBoolean(ConfigurationManager.AppSettings["Enc"].ToString());
                    //if (a)
                    //{
                    //    ProAccEntities db = new ProAccEntities();
                    //    var aa = db.UserMasters.ToList();
                    //    foreach (var item in aa)
                    //    {
                    //        ProAccEntities db1 = new ProAccEntities();
                    //        UserMaster us = new UserMaster();
                    //        us.UserId = item.UserId;
                    //        us.Name = item.Name;
                    //        us.EMail = item.EMail;
                    //        us.Phone = item.Phone;
                    //        us.LoginId = item.LoginId;
                    //        us.Password = _Base.PasswordEncrypt(item.Password);
                    //        us.RoleID = item.RoleID;
                    //        us.UserTypeID = item.UserTypeID;
                    //        us.Customer_Id = item.Customer_Id;

                    //        us.isActive = item.isActive;
                    //        us.IsDeleted = item.IsDeleted;


                    //        us.Cre_on = item.Cre_on;
                    //        us.Cre_By = item.Cre_By;


                    //        us.Modified_by = item.Modified_by;
                    //        us.Modified_On = item.Modified_On;
                    //        db1.Entry(us).State = EntityState.Modified;
                    //        db1.SaveChanges();

                    //    }
                    //}

                    _Log.createLog("------>Before");
                    logedUser = _Base.UserValidation(logedUser);
                    if (logedUser.ID != Guid.Empty)
                    {
                        _Log.createLog("------>After");
                        //_Log.createLog("" + logedUser.ID.ToString() + "------>"+"" + logedUser.Name.ToString() + "------>" + logedUser.Password.ToString());
                        FormsAuthentication.SetAuthCookie(logedUser.Username, false);
                        Session["log-id"] = logedUser.LogID.ToString();
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
                throw ex;
            }
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Forgotpassword()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string username, string emailId)
        {

            // ResetMailLink rm = new ResetMailLink();


            var _userId = (from emp in db.UserMasters
                           where emp.LoginId == username && emp.EMail == emailId && emp.isActive == true
                           select emp.UserId).FirstOrDefault();
            if (_userId != null)
            {
                try
                {
                    Guid UserId = _userId;
                    Guid CreatedBy = _userId;
                    DateTime CreatedOn = DateTime.UtcNow;
                    bool Status = true;
                    bool isActive = true;
                    bool result = _Base.Sp_ResetPasswordStatus(UserId, Status, CreatedBy, CreatedOn, isActive);


                    rt = _Base.Sp_GetResetId(UserId);

                    var link = Url.Action("ResetLink", "Login", new { email = emailId, code = rt.ResetId });
                    //var linkUrl = "/Login/ResetLink?" + "email=" + emailId + "&code=" + rt.ResetId;
                    //string Ab_Path = Request.Url.PathAndQuery;
                    //var url = Request.Url.AbsoluteUri.Replace(Ab_Path+":4041", link);
                      string Server_Path = WebConfigurationManager.AppSettings["Server_Path"];

                    string msg = "<b>Please find the Password Reset Link. </b><br/>" + "<a href='" + Server_Path+link + "'>Click Link To Reset Password</a>";
                    bool s = _Base.AddResetMail(rt.ResetId, UserId, emailId, msg);
                    //await rm.FetchLink(emailId, msg, subj);

                    if (s == true)
                    {
                        return Json("success");
                    }
                    else
                    {
                        return Json("error");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return Json("error");
            }
        }

        [AllowAnonymous]
        public ActionResult ResetLink(string email, string code)
        {
            Guid ResetId = Guid.Parse(code);
            var result = db.ResetPasswords.ToList().Exists(x => x.ResetId == ResetId && x.IsActive == true && x.Status == false);
            if (result)
            {
                if (email != null && code != null)
                {
                    ViewData["Mail"] = email;
                    ViewData["code"] = code;
                    return View();
                }
                else
                {
                    ViewData["Status"] = false;
                    return View();
                    //return Json("error");
                }
            }
            else
            {
                ViewData["Status"] = false;
                return View();
                //return Json("error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetLink(string password, string Mail, string code)
        {
            Boolean flag = false;
            Guid ResetId = Guid.Parse(code);
            try
            {
                Guid _userId = db.ResetPasswords.Where(x => x.IsActive == true && x.Status == false && x.ResetId == ResetId).FirstOrDefault().UserId;

                string User_MailID = db.UserMasters.Where(x => x.isActive == true && x.UserId == _userId).FirstOrDefault().EMail;
                if (Mail == User_MailID)
                {
                    UserMaster D = new UserMaster();
                    D.UserId = _userId;
                    D.Password = _Base.Encrypt(password);
                    D.Modified_by = _userId;
                    D.Modified_On = DateTime.UtcNow;
                    flag = _Base.Sp_ResetPassword(D);


                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
                //var _userId = (from emp2 in db.UserMasters
                //               where emp2.EMail == Mail && emp2.isActive == true
                //               select emp2.UserId).FirstOrDefault();
                //if (_userId != null)
                //{
                //    try
                //    {
                //        UserMaster emp = new UserMaster();
                //        emp.Password = password;
                //        emp.Modified_by = _userId;
                //        emp.Modified_On = DateTime.UtcNow;
                //        emp.isActive = true;

                //        //bool result2 = _Base.Sp_ResetPassword(emp);
                //        bool result2 = false;
                //        if (result2 == true)
                //        {
                //            flag = true;
                //            return Json("success");
                //        }
                //        else
                //        {
                //            return Json("error");
                //        }
                //    }

            }
            catch (Exception ex)
            {
                return Json("error");
                throw ex;

            }
        }

        public ActionResult LoginIdCheck(string username, string mail, string code)
        {
            Guid ResetId = Guid.Parse(code);
            var _userId = (from emp in db.ResetPasswords
                           where emp.ResetId == ResetId && emp.IsActive == true
                           select emp.UserId).FirstOrDefault();
            var result = db.UserMasters.ToList().Exists(x => x.LoginId == username && x.UserId == _userId && x.EMail == mail && x.isActive == true);
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NewPasswordcheck(string password)
        {
            string passwordEncrypt = _Base.Encrypt(password);
            var result = db.UserMasters.ToList().Exists(x => x.Password == passwordEncrypt && x.isActive == true);
            if (result != true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);


            }
        }

        public ActionResult UserNamecheck(string username, string emailId)
        {

            var result = db.UserMasters.ToList().Exists(x => x.LoginId == username && x.EMail == emailId && x.isActive == true);
            if (result != true)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("success", JsonRequestBehavior.AllowGet);

            }
        }

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

    }

}

