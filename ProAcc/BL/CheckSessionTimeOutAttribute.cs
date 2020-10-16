using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProAcc.BL
{
    //[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionTimeOutAttribute: ActionFilterAttribute
    {
        Base _Base = new Base();
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];
                    // "loginid"
                    //if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET&#95;SessionId") >= 0))
                    if (context.Session["loginid"] == null)
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Login/Logout";
                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            redirectTo = string.Format("~/Login/Logout?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
                        }
                        filterContext.HttpContext.Response.Redirect(redirectTo, true);
                    }
                }
                //else if (context.Session["log-id"] != null)
                //{
                //    Boolean Status = false;
                //    Guid logid = Guid.Parse(context.Session["log-id"].ToString());
                //    Status = _Base.Validate_Log(logid);
                //    if (Status == false)
                //    {
                //        //FormsAuthentication.SignOut();
                //        string redirectTo = "~/ErrorLog/Multilogin_Error";
                //        context.Session.Clear();
                //        //if (!string.IsNullOrEmpty(context.Request.RawUrl))
                //        //{
                //        //    redirectTo = string.Format("~/ErrorLog/Multilogin_Error", HttpUtility.UrlEncode(context.Request.RawUrl));
                //        //}
                //        filterContext.HttpContext.Response.Redirect(redirectTo, true);
                //    }
                //}
            }
                base.OnActionExecuting(filterContext);            
        }
    }
}
