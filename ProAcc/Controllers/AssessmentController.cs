﻿using ProAcc.BL;
using ProAcc.BL.Model;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant,Customer")]
    public class AssessmentController : Controller
    {
       
        ProAccEntities db = new ProAccEntities();
        LogHelper _logHelper = new LogHelper();
        Base _Base = new Base();
        private Guid InstanceId = Guid.Empty;
        // GET: Assessment
        [Authorize(Roles = "Admin,Consultant")]
        public ActionResult CreateAnalysis()
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

            //List<SelectListItem> Project = new List<SelectListItem>();
            //var query = from u in db.CustomerProjectConfigs where (u.isActive == true) select u;
            //if (query.Count() > 0)
            //{
            //    foreach (var v in query)
            //    {
            //        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
            //    }
            //}
            //ViewBag.Project = Project;
            return View();
        }

        #region Chart
        public JsonResult GetActivities_Bar1()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetActivities_Bar1(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActivities_Bar2()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetActivities_Bar2(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetActivities_Donut()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetActivities_Donut(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFiori_Bar()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetFiori_Bar(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomCode_Bar()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetCustomCode_Bar(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFioriAppsReportt_Bar(string Input)
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetSAPFioriAppsReport_Bar(Input, InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetActivitiesReport_Bar(string Phase, string condition)
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetActivitiesReport_Bar(Phase, condition, InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSimplificationReport_Bar(string LOB)
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_SimplificationReport_Bar(LOB, InstanceId);

            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
        #endregion




        #region Table
        public ActionResult ReadinessReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString());
                //return RedirectToAction("Home", "Home");
            }
            else
            {

            }
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


            List<SelectListItem> Project = new List<SelectListItem>();

            if (User.IsInRole("Customer"))
            {
                Guid customerId = Guid.Parse(Session["loginid"].ToString());
                var query = from u in db.CustomerProjectConfigs where (u.CustomerID == customerId && u.isActive == true) select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                    }
                }
            }
            else
            {

                var query = from u in db.CustomerProjectConfigs where (u.isActive == true) select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                    }
                }
            }


            ViewBag.Project = Project;
            return View();
        }
        [HttpPost]
        public JsonResult GetSimplificationReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            SP_ReadinessReport_Result GetRelevant = _Base.sAPInput(InstanceId);
            return Json(GetRelevant, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SimplificationReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString()).Replace("\n", Environment.NewLine);

            }
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_SimplificationReport(InstanceId);
            ViewBag.LOB = new SelectList(sP_._List, "_Value", "Name");
            //List<SAPInput_SimplificationReport> SR = _Base.SAPInput_Simplification(InstanceId);
            //ViewBag.SRReport = SR;

            return View();
        }

        public ActionResult CustomReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString()).Replace("\n", Environment.NewLine);

            }
            GeneralList sP_ = _Base.sP_SimplificationReport(InstanceId);
            ViewBag.LOB = new SelectList(sP_._List, "_Value", "Name");
            List<SAPInput_CustomCode> CR = _Base.SAPInput_CustomCodeReport(InstanceId);
            ViewBag.CRReport = CR;
            return View();
        }

        public ActionResult ActivitieReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString()).Replace("\n", Environment.NewLine);

            }
            Tuple<List<Lis>, List<Lis>> sP_ = _Base.sp_GetActivitiesReportDropdown(InstanceId);
            ViewBag.Condition = new SelectList(sP_.Item2, "Value", "Name");
            ViewBag.Phase = new SelectList(sP_.Item1, "Value", "Name");
            List<BL.Model.SAPInput_Activities> AR = _Base.GetActivitiesReport_Table(InstanceId);
            ViewBag.ARReport = AR;
            return View();
        }

        public ActionResult FioriAppsReport()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            //InstanceId = Guid.Parse("d2708e9f-eda4-47c2-b028-2776353ae755");

            if (InstanceId == Guid.Empty)
            {
                ViewBag.Message = String.Format("Hello {0},\n Kindly Select Instance", Session["UserName"].ToString()).Replace("\n", Environment.NewLine);

            }
            GeneralList sP_ = _Base.sp_GetFioriAppsReportDropdown(InstanceId);
            ViewBag.Roles = new SelectList(sP_._List, "_Value", "Name");
            List<SAPFioriAppsModel> FiR = _Base.sp_GetSAPFioriAppsTable(InstanceId);
            ViewBag.FiRReport = FiR;
            return View();
        }



        public ActionResult SimplicationTable(string LOB)
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            List<SAPInput_SimplificationReport> SR = _Base.SAPInput_Simplification(InstanceId,LOB);
            
            return Json(SR, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult ActivitiesTable()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            List<BL.Model.SAPInput_Activities> AC = _Base.GetActivitiesReport_Table(InstanceId);
            
            return Json(AC, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult CustomCodeTable()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            List<SAPInput_CustomCode> CC = _Base.SAPInput_CustomCodeReport(InstanceId);
            
            return Json(CC, JsonRequestBehavior.AllowGet);
        } 
        
        public ActionResult FioriAppsTable()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            List<SAPFioriAppsModel> AC = _Base.sp_GetSAPFioriAppsTable(InstanceId);
            
            return Json(AC, JsonRequestBehavior.AllowGet);
        } 
       


        #endregion





        [HttpPost]
        [Authorize(Roles = "Admin,Consultant")]
        public ActionResult Upload()
        {
            //string Cust_ID = Request.Params["Cust_ID"].ToString();
            string IDProject = Request.Params["IDProject"].ToString();
            string InstanceName = Request.Params["InstanceID"].ToString();

            if (IDProject != null)
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        bool Result_Process_Activities = false, Result_Process_Bwextractors = false,
                            Result_Process_CustomCode = false, Result_Processup_HanaDatabaseTables = false,
                            Result_Process_FioriApps = false, Result_Process_Simplification = false,
                            Result_Process_SAPReadinessCheck = false;
                        Guid Instance_ID = Guid.Parse(InstanceName);
                        Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                        //  Get all files from Request object  
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            int FileCount = 0;
                            FileCount = Convert.ToInt32(files.AllKeys[i]) + 1;
                            HttpPostedFileBase file = files[i];
                            string fname;
                            string ext;
                            string NewID = Guid.NewGuid().ToString();
                            //Checking for Internet Explorer
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                                ext = System.IO.Path.GetExtension(fname);
                            }
                            else
                            {
                                fname = file.FileName;
                                ext = System.IO.Path.GetExtension(fname);
                            }
                            String _fname = NewID + ext;
                            FileUpload _fileUpload = new FileUpload();
                            string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                            _Base.CreateIfMissing(Folder_Path);
                            fname = Path.Combine(Folder_Path, _fname);
                            file.SaveAs(fname);
                            if (FileCount == 1)
                            {
                                Result_Process_Activities = _fileUpload.Process_Activities(fname, NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 2)
                            {
                                Result_Process_Bwextractors = _Base.Upload_Bwextractors(NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 3)
                            {
                                Result_Process_CustomCode = _fileUpload.Process_CustomCode(fname, NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 4)
                            {
                                Result_Processup_HanaDatabaseTables = _Base.Upload_HanaDatabaseTables(NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 5)
                            {
                                Result_Process_FioriApps = _fileUpload.Process_FioriApps(fname, NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 6)
                            {
                                Result_Process_Simplification = _fileUpload.Process_Simplification(fname, NewID, Instance_ID, User_Id);
                            }
                            else if (FileCount == 7)
                            {
                                Result_Process_SAPReadinessCheck = _Base.Upload_SAPReadinessCheck(NewID, Instance_ID, User_Id);
                            }


                        }

                        //if (Result_Process_Bwextractors & Result_Process_Bwextractors &
                        //    Result_Process_CustomCode & Result_Processup_HanaDatabaseTables &
                        //    Result_Process_FioriApps & Result_Process_Simplification &
                        //    Result_Process_SAPReadinessCheck)
                        //{

                        //    //Result_Instance = _Base.AddInstance(IDProject, InstanceName, Instance_ID);

                        //    return Json("File Uploaded Successfully!");
                        //}
                        return Json("File Uploaded Successfully!");
                        // Returns message that successfully uploaded  

                    }
                    catch (Exception ex)
                    {
                        _logHelper.createLog(ex);
                        String msg = ex.Message;
                        if (msg.Contains("Activities"))
                        {

                        }
                        else if(msg.Contains("Activities"))
                        {

                        }
                        else if (msg.Contains("Activities"))
                        {

                        }
                        else if (msg.Contains("Activities"))
                        {

                        }
                        else if (msg.Contains("Activities"))
                        {

                        }
                        else if (msg.Contains("Activities"))
                        {

                        }
                        else if (msg.Contains("Activities"))
                        {

                        }
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("Select ProjectID");
                }
            }
            else
            {
                return Json("No files selected.");
            }


        }

        [HttpPost]
        public JsonResult LoadProject(string CustomerId)
        {
            
            List<SelectListItem> Project = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(CustomerId))
            {
                Guid IDCustomer = Guid.Parse(CustomerId);
                var query = from u in db.CustomerProjectConfigs where (u.CustomerID == IDCustomer && u.isActive == true) select u;
                if (query.Count() > 0)
                {
                    foreach (var v in query)
                    {
                        Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                    }
                }
            }
            return Json(Project, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult LoadInstance(string ProjectId)
        {
            GeneralList sP_ = _Base.GetInstanceDropdown(ProjectId);
            SelectList items = new SelectList(sP_._List, "Value", "Name");
            return Json(items, JsonRequestBehavior.AllowGet);
        }



        
    }
}