using MySql.Data.MySqlClient;
using ProAcc.BL;
using ProAcc.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProAcc.Controllers
{
    public class CustomerQuestionnaireController : Controller
    {
        // GET: CustomerQuestionnaire
        public ActionResult Index()
        {
            return View();
        }

        MySqlConnection con1 = new MySqlConnection();
        Base _Base = new Base();
        public ActionResult CustomerNeedHelpIndex()
        {
            con1.ConnectionString = _Base.Decrypt(ConfigurationManager.ConnectionStrings["MysqlPath"].ConnectionString);
            MySqlDataAdapter da = new MySqlDataAdapter("select * from users where active=1 AND Need_Help=1", con1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<CustomerQuestionnaireViewModel> cqList = new List<CustomerQuestionnaireViewModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CustomerQuestionnaireViewModel cq = new CustomerQuestionnaireViewModel();
                cq.User_ID= Convert.ToInt32(dr["User_ID"].ToString());
                cq.First_Name = dr["First_Name"].ToString();
                cq.Second_Name = dr["Second_Name"].ToString();
                cq.Email = dr["Email"].ToString();
                cq.Phone_No = dr["Phone_No"].ToString();
                cq.Need_Help = Convert.ToBoolean(dr["Need_Help"]);
                cqList.Add(cq);
            }

            return View(cqList);
        }

        public JsonResult UpdateNeepHelp(int id)
        {
            con1.ConnectionString = _Base.Decrypt(ConfigurationManager.ConnectionStrings["MysqlPath"].ConnectionString);
            con1.Open();
            string q = "update users set Need_Help=0 WHERE User_ID = " + id + " ";
            MySqlCommand cmd3 = new MySqlCommand(q, con1);
            cmd3.ExecuteNonQuery();
            con1.Close();
            return Json("success",JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendMailToCustomer()
        {
            return View();
        }

        public JsonResult UpdateSendMailToCustomer(String Name,String Email)
        {
            var mm = _Base.sendmailtocustomer(Name,Email);
            if(mm==true)
            {
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
    }
}