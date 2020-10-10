using iTextSharp.text;
using iTextSharp.text.pdf;
using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]
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
                            orderby b.Project_Name
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
                            orderby b.Project_Name
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
            //var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AssessmentReport()
        {
            Guid InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            ViewBag.Status = true;
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
                //if (File.Exists(PathPdf))
                //{
                //    ViewBag.Status = true;
                //}
                //else
                //{
                Boolean Result = P.convertDOCtoPDF(PathDoc, PathPdf);
                ViewBag.Status = Result;
                // }


                //P.convertDOCtoPDF(PathDoc, PathPdf);
                ViewBag.PDFfileName = ConfigurationManager.AppSettings["Upload_filePath_Temp"].ToString() + "/Pdf/" + fileName + ".pdf";
            }


            return View();
        }


        //public ActionResult AuditReport()
        //{
        //    return View(); 
        //}

        public ActionResult AuditReports()
        {
            //List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditDatas();
            //var obj = new { data = PM };
            //return View(PM);
            return View();
        }

        [HttpGet]
        public ActionResult GetAuditDatas(AuditReport.ProjectMonitorModel model)
        {
            List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditDatas(model);
            //var obj = new { data = PM };
            return PartialView("_AuditReportIndex", PM);
        }

        public ActionResult GetAduitData()
        {
            List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditData();
            var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetAduitDatas()
        //{
        //    List<AuditReport.ProjectMonitorModel> PM = _Base.Sp_GetAuditDatas();
        //    var obj = new { data = PM };
        //    return Json(PM, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult GetDataReport(string Instance)
        {
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            string LoginID = Session["loginid"].ToString();
            List<ProjectMonitorModel> PM = _Base.Sp_GetReportDataReport(InstanceID, LoginID);
            var obj = new { data = PM };
            return Json(PM, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PMReport()
        {
            ViewBag.PDFfilepath = ConfigurationManager.AppSettings["Upload_filePath"].ToString();
            return View();
        }

        public JsonResult PMReportData()
        {
            Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
            List<FileUploadMaster> PM = _Base.GetPMuploadlist(InstanceID);
            var obj = new { data = PM };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult pdfData(bool? pdf)
        {
            if (!pdf.HasValue)
            {
                return View();
            }
            else
            {
                string filePath = Server.MapPath(ConfigurationManager.AppSettings["Sample_pdfPath"].ToString());  //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\Uploadedppt\\Sample.pdf";//Server.MapPath("Content") + "Sample.pdf";
                string imagePath = Server.MapPath(ConfigurationManager.AppSettings["Logo_Path"].ToString()); //"F:\\GitProAccNew\\ProAccNew\\ProAcc\\Asset\\images\\promantus-new-logo.PNG";
                Guid InstanceID = Guid.Parse(Session["InstanceId"].ToString());
                string LoginID = Session["loginid"].ToString();
                String ProjectName= Session["Project_Name"].ToString();
                String InstancetName = Session["Instance_Name"].ToString();
                List<ProjectMonitorModel> PM = _Base.Sp_GetReportDataReportPDF(InstanceID, LoginID);
                String description = "Project Name :"+ ProjectName + " & Instance Name : "+ InstancetName + ".";
                ExportPDF(PM, filePath, imagePath, description);
                return File(filePath, "application/pdf", "DetailedReport.pdf");
            }
        }

        private static void ExportPDF(List<ProjectMonitorModel> PM, string filePath,string imagePath,string description)
        {
            Font headerFont = FontFactory.GetFont("arial", 6,Font.BOLD,BaseColor.BLACK);
            Font rowfont = FontFactory.GetFont("arial", 6);
            Document document = new Document(PageSize.A4.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document,
                       new FileStream(filePath, FileMode.OpenOrCreate));
            document.Open();

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            img.Alignment = Element.ALIGN_LEFT;
            img.ScaleToFit(120f, 170f);
            img.SpacingBefore = 10f;
            img.SpacingAfter = 50f;
            document.Add(img);

            Paragraph para = new Paragraph(description);
            para.Alignment = Element.ALIGN_LEFT;
            para.SpacingAfter = 20f;
            document.Add(para);

            string[] columns = { "BuldingBlock", "Phase", "Task", "ApplicationArea", "Delay_occurred_Report", "Owner", "Status","EST_hrs", "Actual_St_hrs", "PlanedDate", "ActualDate", "PlanedEn_Date", "ActualEn_Date", "Notes" };
            string[] columnHeadings = { "Bulding Block", "Phase", "Task", "Application Area", "Delay", "Owner", "Status","EST (hrs)", "Actual (hrs)", "Planed Start", "Actual Start", "Planed End", "Actual End", "Commnets" };
            PdfPTable table = new PdfPTable(columns.Length)
            { WidthPercentage = 100, RunDirection = PdfWriter.RUN_DIRECTION_LTR, ExtendLastRow = false };
            table.PaddingTop = 300f;
            table.HorizontalAlignment = Element.ALIGN_CENTER;
            table.TotalWidth = 550f;
            float[] widths = new float[] {65f, 45f, 100f, 50f, 25f,40f,40f,25f,25f,30f,30f, 30f, 30f, 50f };
            table.SetWidths(widths);
            foreach (var column in columnHeadings)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column, headerFont));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                table.AddCell(cell);
            }

            foreach (var item in PM)
            {
                foreach (var column in columns)
                {
                    string value = item.GetType().GetProperty(column).GetValue(item).ToString();
                    PdfPCell cell5 = new PdfPCell(new Phrase(value, rowfont));
                    table.AddCell(cell5);
                }
            }
            document.Add(table);
            document.Close();
        }
    }
}