using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    public class ReportController : Controller
    {
        Base _Base = new Base();
        private ProAccEntities db = new ProAccEntities();
        // GET: Report
        public ActionResult OverAllTaskReport()
        {
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
                var q = from u in db.Instances where (u.Instance_id == InstanceID && u.AssessmentUploadStatus == true && u.isActive == true) select u;
                if (q.Count() > 0)
                {
                    inst = 1;
                }
                else { inst = 0; }

            }
            ViewBag.Instance = inst;
            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.Customer_Id equals b.Customer_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            else if (User.IsInRole("Project Manager"))
            {
                Guid LoginId = Guid.Parse(Session["loginid"].ToString());
                var Data = (from a in db.UserMasters
                            join b in db.Projects on a.UserId equals b.ProjectManager_Id
                            where a.UserId == LoginId && b.isActive == true
                            select new { b.Project_Id, b.Project_Name }).ToList();
                if (Data.Count() > 0)
                {
                    foreach (var v in Data)
                    {
                        Project.Add(new SelectListItem { Text = v.Project_Name, Value = v.Project_Id.ToString() });
                    }

                }
            }
            ViewBag.Project = Project;
            return View();
        }

        public ActionResult GetData()
        {
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetReportData(InstanceID, LoginID);
            var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AssessmentReport()
        {
            Guid InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString());
                ViewBag.PDFfileName = "";
                //return RedirectToAction("Home", "Home");
            }
            else
            {
                //Boolean A= _Base.Proceess_WordAddImage();
                //Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
                var fileName = (from u in db.FileUploadMasters
                where
                u.InstanceID == InstanceId &&
                u.isActive == true &&
                u.File_Type == _Base.SAPReportFileName// "SAPReadinessCheck"
                select u.C_FileName).FirstOrDefault();

                CreatePDF P = new CreatePDF();
                string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                string TempPath = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath_Temp"].ToString());
                string PathPdf = TempPath + "\\Pdf\\" + fileName + ".pdf";
                string PathDoc = Folder_Path + fileName + ".docx";
                P.SpireconvertDOCtoPDF(PathDoc, PathPdf);
                //P.convertDOCtoPDF(PathDoc, PathPdf);
                ViewBag.PDFfileName = ConfigurationManager.AppSettings["Upload_filePath_Temp"].ToString() + "/Pdf/" + fileName + ".pdf";
            }
            

            return View();
        }


        public ActionResult AuditReport()
        {
            return View(); 
        }

        public ActionResult GetAduitData()
        {
            List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditData();
            var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }


    }
}