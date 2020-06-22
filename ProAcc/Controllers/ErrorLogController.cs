using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProAcc.Controllers
{
    public class ErrorLogController : Controller
    {
        // GET: ErrorLog
        public ActionResult Error404()
        {
            return View();
        }
    }
}