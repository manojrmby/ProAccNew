using ProAcc.BL;
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
    [Authorize(Roles = "Admin,Consultant,Customer,Project Manager")]
    public class LicenseController : Controller
    {
        // GET: Licence
        Base _Base = new Base();
        LogHelper _logHelper = new LogHelper();
        private Guid InstanceId = Guid.Empty;
        private ProAccEntities db = new ProAccEntities();
        public ActionResult License()
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
            //ViewBag.Customer = new SelectList(sP_._List, "Value", "Name");
            TempData["Customer"] = new SelectList(sP_._List, "Value", "Name");
            //TempData["PhaseID"] = (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
            //ViewBag.PhaseID= (from q in db.PhaseMasters where q.PhaseName == _Base.Phase_Assessment && q.isActive == true select q.Id).FirstOrDefault();
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
            //ViewBag.Instance = inst;
            //TempData["Instance"] = inst;
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
            //ViewBag.Project = Project;
            TempData["Project"] = Project;
            return View();
        }


        [HttpPost]
        public ActionResult Lic_Upload()
        {
            string InstanceName = Request.Params["InstanceID"].ToString();
            string filetype = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    bool Result_Process = false;
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
                        if (FileCount == 1)//9
                        {
                            filetype = "UserList";
                            Result_Process = _fileUpload.Process_Userlist(fname, NewID, Instance_ID, User_Id);
                        }
                        //Result_Process = _Base.Lic_Upload(filetype, NewID, Instance_ID, User_Id);
                    }
                    return Json("File Uploaded Successfully!");

                }
                catch (Exception ex)
                {
                    _logHelper.createLog(ex);
                    String msg = ex.Message;
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("Please choose at least one file...");
            }
        }

        public JsonResult GetLicense_Bar()
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            GeneralList sP_ = _Base.sP_GetLicense_Bar(InstanceId);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetLic_Data(int value)
        {
            InstanceId = Guid.Parse(Session["InstanceId"].ToString());
            List<SAPInput_UserList> sP_ = _Base.sP_GetLicense_Table(InstanceId, value);
            return Json(sP_, JsonRequestBehavior.AllowGet);
        }
    }
}