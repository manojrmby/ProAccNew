using ProAcc.BL;
using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ProAcc.BL.Model.Common;
using ProAcc.BL.Model;
using Newtonsoft.Json;

namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin,Consultant")]
    public class PreConvertionController : Controller
    {
        Base _Base = new Base();
        LogHelper _logHelper = new LogHelper();
        ProAccEntities db = new ProAccEntities();
        private Guid InstanceId = Guid.Empty;
        // GET: PreConvertion
        public ActionResult PreConvertion()
        {
            //Gokul
            return View();
        }

        public ActionResult PreConvertion_Upload()
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

            List<SelectListItem> Project = new List<SelectListItem>();
            var query = from u in db.CustomerProjectConfigs where (u.isActive == true) select u;
            if (query.Count() > 0)
            {
                foreach (var v in query)
                {
                    Project.Add(new SelectListItem { Text = v.ProjectName, Value = v.Id.ToString() });
                }
            }
            ViewBag.Project = Project;
            return View();
        }


       


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
                        bool Result_Process_PreConvertion = false;

                        Guid User_Id = Guid.Parse(Session["loginid"].ToString());
                        Guid Instance_ID = Guid.Parse(InstanceName);
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
                            FileUpload_Pic _fileUpload = new FileUpload_Pic();
                            string Temp_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath_Temp"].ToString());
                            _Base.CreateIfMissing(Temp_Path);
                            string SourcePath = Server.MapPath(ConfigurationManager.AppSettings["Upload_filePath"].ToString());
                            _Base.CreateIfMissing(SourcePath);
                            // Get the complete folder path and store the file inside it.  
                            fname = Path.Combine(SourcePath, _fname);
                            file.SaveAs(fname);
                            if (FileCount == 1)
                            {
                                Result_Process_PreConvertion = _fileUpload.Pre_Process_PreConvertion(fname, NewID, Instance_ID,Temp_Path, User_Id);
                            }
                            


                        }

                        return Json("File Uploaded Successfully!");

                    }
                    catch (Exception ex)
                    {
                        _logHelper.createLog(ex);
                        String msg = ex.Message;
                        if (msg.Contains("Activities"))
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




        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EditTable()
        { return View(); }

       
        public ActionResult GetData()
        {
            //InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            InstanceId = Guid.Parse("d9747ed5-f482-4ac2-9ef9-4a25aa5a6855");
            List<SAPInput_PreConvertion> CR = _Base.sp_GetPreConvertionTable(InstanceId);
            var obj = new { data = CR };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditGetData()
        {
            
                    //InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            InstanceId = Guid.Parse("d9747ed5-f482-4ac2-9ef9-4a25aa5a6855");
            List<SAPInput_PreConvertion> CR = _Base.sp_GetPreConvertionTable(InstanceId);
            var obj = new { data = CR };
            return Json(CR, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditSubmit(SAPInput_PreConvertion Data)
        {
            Boolean Status = false;
            Status= _Base.sp_PreConvertion_Update(Data);
            //InstanceId = Guid.Parse("d9747ed5-f482-4ac2-9ef9-4a25aa5a6855");
            //List<SAPInput_PreConvertion> CR = _Base.sp_GetPreConvertionTable(InstanceId);
            //return Json(CR, JsonRequestBehavior.AllowGet);
            return Json(Status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPicturetoDatas()
        {

            List<PicturetoData> CR = _Base.Sp_GetPicturetoDatas();
            
            return Json(CR, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadInstance(string ProjectId)
        {
            GeneralList sP_ = _Base.GetPerConvertionUploadInstance(ProjectId);
            SelectList items = new SelectList(sP_._List, "Value", "Name");
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}