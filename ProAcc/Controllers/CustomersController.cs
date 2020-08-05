using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProAcc.BL.Model;
using ProACC_DB;
using PagedList;
using PagedList.Mvc;
using ProAcc.BL;
using static ProAcc.BL.GetQuestionary;
using Spire.Presentation;
using System.Text.RegularExpressions;
using System.Configuration;
using System.IO;


namespace ProAcc.Controllers
{
    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class CustomersController : Controller
    {
        private ProAccEntities db = new ProAccEntities();
        LogHelper _Log = new LogHelper();
        Base _Base = new Base();
        // GET: Customers

        public ActionResult LinkPage()
        {
            return View();
        }
        public ActionResult Index()
        {
            List<ProACC_DB.Customer> customers = null; 
            try
            {
               customers = db.Customers
              .Where(a => a.isActive == true)
              .OrderByDescending(x => x.Cre_on).ToList();
                //.Where(x => x.Name.StartsWith(search) || search == null).ToList(); //.ToPagedList(i ?? 1, 5);
                //return View(customers);
                //ViewBag.customersIndex = db.Customers.Where(x => x.isActive == true).OrderByDescending(x => x.Cre_on).ToList();
                //return View(ViewBag.customersIndex);
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->Customers Index" + ex.Message.ToString());
            }
            return View(customers);
        }
        // GET: Customers/Details/5
        public ActionResult Details(Guid? id)
        {
            Customer customer = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->Customers Details" + ex.Message.ToString());
            }
           
            return View(customer);
        }

        public JsonResult CheckCustomersNameAvailability(string namedata,Guid? id)
        {
            if(id!=null)
            {
                var SearchDt = db.Customers.Where(x => x.Company_Name == namedata).Where(x=>x.Customer_ID!=id).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var SearchDt = db.Customers.Where(x => x.Company_Name == namedata).Where(x => x.isActive == true).FirstOrDefault();
                if (SearchDt != null)
                {
                    return Json("error", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
            }
            
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.IndustrySector = db.IndustrySectors.Where(x => x.IsActive == true);
            ViewBag.customersIndex = db.Customers.Where(x => x.isActive == true).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult GetCustomers()
        {
            List<ProACC_DB.Customer> list = null;
            try
            {
                list = db.Customers.Where(x => x.isActive == true).ToList();
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->GetCustomers" + ex.Message.ToString());
            }
           
            return PartialView("_CustomerIndex", list);
        }
        
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                var name = db.Customers.Where(p => p.Company_Name == customer.Company_Name).Where(x => x.isActive == true).ToList();
                if (name.Count==0)
                {
                    customer.Customer_ID = Guid.NewGuid();
                    customer.isActive = true;
                    customer.Cre_on = DateTime.UtcNow;
                    customer.Cre_By = Guid.Parse(Session["loginid"].ToString());
                    customer.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    customer.Modified_On = DateTime.UtcNow;
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return Json("success");                    
                }
                else
                {
                    return Json("error");
                }                
                
            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, "-->Create Post" + Url);
                throw;
            }
            
        }

        public ActionResult GetCustomerById(Guid? id)
        {
            Customer customer = null;
            try
            {               
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                var Data = db.Customers.Find(id);
                ViewBag.IndustrySector = db.IndustrySectors.Where(x => x.IsActive == true);
                //Customer cust = new Customer();
                //cust.Customer_ID = Data.Customer_ID;
                //cust.Company_Name = Data.Company_Name;
                //cust.Contact = Data.Contact;
                //cust.Phone = Data.Phone;
                //cust.Email = Data.Email;
            }
            catch (Exception Ex)
            {
                _Log.createLog(Ex, "-->Create Custoemr Post" + Url);
            }

            return View(customer);
            //return Json(cust, JsonRequestBehavior.AllowGet);
        }

        // GET: Customers/Edit/5
        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var val = db.User_Type.Where(a => a.isActive == true);
           
        //    return View(customer);
        //}

     
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            try
            {
                var name = db.Customers.Where(p => p.Company_Name == customer.Company_Name).Where(x => x.Customer_ID != customer.Customer_ID).Where(x => x.isActive == true).ToList();
                if (name.Count == 0)
                {
                    customer.Modified_On = DateTime.UtcNow;
                    //customer.Cre_on = DateTime.Now;
                    customer.Modified_by = Guid.Parse(Session["loginid"].ToString());
                    customer.isActive = true;
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, "-->Edit Customer Post" + Url);
                throw;
            }
           
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            Customer customer = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                 customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
            }
            catch(Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, "-->Delete Customer" + Url);
                throw;
            }
          
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var del = (from a in db.Customers
                           join b in db.Projects
                           on a.Customer_ID equals b.Customer_Id
                           where a.Customer_ID == id && a.isActive == true && b.isActive == true
                           select b).ToList();

                if (del.Count != 0)
                {
                    return Json("fail");
                }
                else
                {
                    Customer customer = db.Customers.Find(id);
                    if (customer.Customer_ID == id)
                    {
                        customer.isActive = false;
                        customer.IsDeleted = true;
                        customer.Modified_On = DateTime.UtcNow;
                        customer.Modified_by = Guid.Parse(Session["loginid"].ToString());
                        db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                return Json("success");
            }
            catch (Exception Ex)
            {
                string Url = Request.Url.AbsoluteUri;
                _Log.createLog(Ex, "-->Delete Customer Post" + Url);
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Downloadppt(Guid id)
        {
            
            question p = _Base.Downloadppt();

            //question p = new question();
            string pattern = "(\")";
            string input = p.la_q2;
            string la_q2_1 = "", la_q2_2 = "", la_q2_3 = "", la_q2_4 = "", la_q2_5 = "", la_q2_6 = "";

            Regex regex = new Regex(pattern);
            string[] substrings = regex.Split(input);
            la_q2_1 = substrings[6].ToString();
            la_q2_2 = substrings[14].ToString();
            la_q2_3 = substrings[22].ToString();
            la_q2_4 = substrings[30].ToString();
            la_q2_5 = substrings[38].ToString();
            la_q2_6 = substrings[46].ToString();

            string input1 = p.la_q4;
            string la_q4_1 = "", la_q4_2 = "", la_q4_3 = "", la_q4_4 = "";
            string[] substring = regex.Split(input1);
            la_q4_1 = substring[6].ToString();
            la_q4_2 = substring[14].ToString();
            la_q4_3 = substring[22].ToString();
            la_q4_4 = substring[30].ToString();


            //Create a Presentation instance
            Presentation ppt = new Presentation();
            string file1 = Server.MapPath(ConfigurationManager.AppSettings["Upload_pptPath"].ToString());
            string file = Path.Combine(file1, "Defaultppt/Default_ProAcc.pptx");
            //@"C:/Users/promantus inc/Downloads/ProAcc_Assessment.pptx";

            int numberOfSlides = CountSlides(file);

            //System.Console.WriteLine("Number of slides = {0}", numberOfSlides);
            //Load the PowerPoint document
            ppt.LoadFromFile(file);


            var Company_Name = db.Customers.Where(x => x.isActive == true && x.Customer_ID == id).FirstOrDefault().Company_Name;
            for (int i = 0; i < numberOfSlides; i++)
            {
                ISlide slide = ppt.Slides[i];
                slide.ReplaceAllText("XXXXX", "Promantus", false);
                slide.ReplaceAllText("XXXX", Company_Name, false);
                slide.ReplaceAllText("_la_q1", p.la_q1, false);
                slide.ReplaceAllText("_la_q2_1", la_q2_1, false);
                slide.ReplaceAllText("_la_q2_2", la_q2_2, false);
                slide.ReplaceAllText("_la_q2_3", la_q2_3, false);
                slide.ReplaceAllText("_la_q2_4", la_q2_4, false);
                slide.ReplaceAllText("_la_q2_5", la_q2_5, false);
                slide.ReplaceAllText("_la_q2_6", la_q2_6, false);
                slide.ReplaceAllText("_la_q3", p.la_q3, false);
                slide.ReplaceAllText("_la_q4_1", la_q4_1, false);
                slide.ReplaceAllText("_la_q4_2", la_q4_2, false);
                slide.ReplaceAllText("_la_q4_3", la_q4_3, false);
                slide.ReplaceAllText("_la_q4_4", la_q4_4, false);
                slide.ReplaceAllText("_la_q5", p.la_q5, false);
                slide.ReplaceAllText("_la_q6", p.la_q6, false);
            }
            String _fname = Company_Name+".pptx";
            string Folder_Path = Server.MapPath(ConfigurationManager.AppSettings["Upload_pptPath"].ToString());
            string fname = Path.Combine(Folder_Path, _fname);
            ViewBag.pptfileName = fname;
            TempData["mydata"] = fname;
            ppt.SaveToFile(fname, FileFormat.Pptx2013);
            
            ///////////////
            //ppt.SaveToFile(fname, FileFormat.PDF);
            //byte[] fileBytes = System.IO.File.ReadAllBytes(fname);
            //string fileName = "PROACC.pptx";
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            //System.Diagnostics.Process.Start(fname);
            ////////////////////
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        private static int CountSlides(string presentationFile)
        {
            using (Presentation presentation = new Presentation(presentationFile, FileFormat.Pptx2010))
            {
                return presentation.Slides.Count;
            }

        }

        public ActionResult DownloadAttachment()
        {

            var data = TempData["mydata"].ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(data);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, data);

        }
    }
}
