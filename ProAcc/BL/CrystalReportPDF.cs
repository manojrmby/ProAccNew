using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using MySql.Data.MySqlClient;
using ProAcc.Asset.images;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace ProAcc.BL
{
    public class CrystalReportPDF
    {
        private readonly string _PDFRunStatus = WebConfigurationManager.AppSettings["PDFRunStatus"];
        LogHelper _log = new LogHelper();
        public void Report()
        {

            try
            {
                Boolean PDFRunStatus = Convert.ToBoolean(_PDFRunStatus);
                if(PDFRunStatus)
                {
                    PDFGeneration();
                }
            }
            catch (Exception ex)
            {
                _log.createLog(ex, "-->CrystalReportPDF" + ex.Message.ToString());
            }
        }

        public void PDFGeneration()
        {
            MySqlConnection con1 = new MySqlConnection();
            //SqlConnection con1 = new SqlConnection();
            //con1.ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
            //con1.ConnectionString = "Data Source = 123.176.34.15;port=4043;Integrated Security=False; Initial Catalog = survey; User ID = TestLogin; Password = ASD123!@#;";
            con1.ConnectionString = ConfigurationManager.ConnectionStrings["MysqlPath"].ConnectionString;
            //SqlDataAdapter da = new SqlDataAdapter("select * from question where submitted =1 AND MailStatus=0", con1);
            MySqlDataAdapter da = new MySqlDataAdapter("select * from question where submitted =1 AND MailStatus=0 and id not in (1,2,3)", con1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Base @base = new Base();



            foreach (DataRow dr in dt.Rows)
            {
                ReportDocument ss = new ReportDocument();
                ConnectionInfo myConnectionInfo = new ConnectionInfo();
                myConnectionInfo.AllowCustomConnection = true;
                myConnectionInfo.ServerName = ConfigurationManager.AppSettings["SERVERNAME"].ToString();
                myConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DATABASE"].ToString();
                myConnectionInfo.IntegratedSecurity = Convert.ToBoolean(ConfigurationManager.AppSettings["INTEGRATEDSECURITY"].ToString());
                myConnectionInfo.UserID = ConfigurationManager.AppSettings["USERID"].ToString();
                myConnectionInfo.Password = ConfigurationManager.AppSettings["PASSWORD"].ToString();
                myConnectionInfo.Type = ConnectionInfoType.SQL;

                string Rpt_Path = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["rptLocation"].ToString());
                string outputPath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["OutputPath_pdf"].ToString());

                try
                {

                    JavaScriptSerializer scr = new JavaScriptSerializer();
                    Question2 Que2 = (Question2)scr.Deserialize(dr["la_q2"].ToString().Replace("\\", ""), typeof(Question2));
                    Question4 Que4 = (Question4)scr.Deserialize(dr["la_q4"].ToString().Replace("\\", ""), typeof(Question4));
                    Question7 Que7 = (Question7)scr.Deserialize(dr["la_q7"].ToString().Replace("\\", ""), typeof(Question7));
                    question8 Que8 = (question8)scr.Deserialize(dr["la_q8"].ToString().Replace("\\", ""), typeof(question8));
                    List<string> Que9 = (List<string>)scr.Deserialize(dr["la_q9"].ToString().Replace("\\", ""), typeof(List<string>));
                    Question11 Que11 = (Question11)scr.Deserialize(dr["la_q11"].ToString().Replace("\\", ""), typeof(Question11));
                    Question15 Que15 = (Question15)scr.Deserialize(dr["la_q15"].ToString().Replace("\\", ""), typeof(Question15));
                    FQ1 fq1 = (FQ1)scr.Deserialize(dr["fq1"].ToString().Replace("\\", ""), typeof(FQ1));
                    FQ2 fq2 = (FQ2)scr.Deserialize(dr["fq2"].ToString().Replace("\\", ""), typeof(FQ2));
                    FQ3 fq3 = (FQ3)scr.Deserialize(dr["fq3"].ToString().Replace("\\", ""), typeof(FQ3));


                    string que1 = dr["la_q1"].ToString();
                    string que3 = dr["la_q3"].ToString();
                    string que5 = dr["la_q5"].ToString();
                    string que6 = dr["la_q6"].ToString();
                    string que10 = dr["la_q10"].ToString();
                    string que12 = dr["la_q12"].ToString();
                    string que13 = dr["la_q13"].ToString();
                    string que14 = dr["la_q14"].ToString();

                    String User_ID = dr["User_ID"].ToString();

                    CrystalReport crypt = new CrystalReport();

                    crypt.SetParameterValue("Q4.1", Que4.la_q4_1);
                    crypt.SetParameterValue("Q4.2", Que4.la_q4_2);
                    crypt.SetParameterValue("Q4.3", Que4.la_q4_3);

                    crypt.SetParameterValue("Q6", que6);

                    crypt.SetParameterValue("Q11", Que11.la_q11_1.ToString() + " , " + Que11.la_q11_2.ToString());

                    crypt.SetParameterValue("Q13", que13);

                    crypt.SetParameterValue("Q14", que14);

                    crypt.SetParameterValue("Q15", Que15.la_q15_1 + " , " + Que15.la_q15_2.ToString());

                    crypt.SetParameterValue("f1", fq1.f1);
                    crypt.SetParameterValue("f2", fq1.f2);
                    crypt.SetParameterValue("f3", fq1.f3);
                    crypt.SetParameterValue("f4", fq2.f4);
                    crypt.SetParameterValue("f5", fq2.f5);
                    crypt.SetParameterValue("f6", fq2.f6);
                    crypt.SetParameterValue("f7", fq3.f7);
                    crypt.SetParameterValue("f8", fq3.f8);
                    if(que3=="no")
                    {
                        if(Que4.la_q4_1!= "SAP ECC6.0" && Que4.la_q4_1 != "EHP")
                        {
                            crypt.SetParameterValue("Img_Op1", true); 
                            crypt.SetParameterValue("Img_Op2", false); 
                        }
                        else
                        {
                            crypt.SetParameterValue("Img_Op2", true);
                            crypt.SetParameterValue("Img_Op1", false);
                        }
                    }

                    foreach (CrystalDecisions.CrystalReports.Engine.Table myTable in crypt.Database.Tables)
                    {
                        TableLogOnInfo myTableLogonInfo = myTable.LogOnInfo;
                        myTableLogonInfo.ConnectionInfo = myConnectionInfo;
                        myTable.ApplyLogOnInfo(myTableLogonInfo);
                        myTable.Location = myConnectionInfo.DatabaseName + ".dbo." + myTable.Location.Substring(myTable.Location.LastIndexOf(".") + 1);
                        myTable.LogOnInfo.ConnectionInfo.ServerName = myConnectionInfo.ServerName;
                    }

                    CrystalReportViewer crb = new CrystalReportViewer();
                    crb.ReportSource = crypt;


                    ss.Load(Rpt_Path);


                    #region Q2
                    String Q2 = "";
                    if (Que2.la_q2_1 != "")
                    {
                        Q2 = Que2.la_q2_1 + ",";
                    }

                    if (Que2.la_q2_2 == "yes")
                    {
                        Q2 = Q2 + " Development,";
                    }

                    if (Que2.la_q2_3 == "yes")
                    {
                        Q2 = Q2 + " Sandbox,";
                    }

                    if (Que2.la_q2_4 == "yes")
                    {
                        Q2 = Q2 + " Quality / Test,";
                    }

                    if (Que2.la_q2_5 == "yes")
                    {
                        Q2 = Q2 + " Production,";
                    }

                    if (Que2.la_q2_6 == "yes")
                    {
                        Q2 = Q2 + " Any other System / Client in the SAP Landscape";
                    }

                    #endregion Q2
                    ss.SetParameterValue("Q2", Q2);

                    ss.SetParameterValue("Q4.1", Que4.la_q4_1);
                    ss.SetParameterValue("Q4.2", Que4.la_q4_2);
                    ss.SetParameterValue("Q4.3", Que4.la_q4_3);

                    ss.SetParameterValue("Q6", que6);

                    if (que3 == "no")
                    {
                        if (Que4.la_q4_1 != "SAP ECC6.0" && Que4.la_q4_1 != "EHP")
                        {
                            ss.SetParameterValue("Img_Op1", true);
                            ss.SetParameterValue("Img_Op2", false);
                        }
                        else
                        {
                            ss.SetParameterValue("Img_Op2", true);
                            ss.SetParameterValue("Img_Op1", false);
                        }
                    }

                    #region Q7
                    String Q7 = "";
                    if (Que7.la_q7_FI == "yes")
                    {
                        Q7 = Q7 + "Financials,";
                    }
                    if (Que7.la_q7_CO == "yes")
                    {
                        Q7 = Q7 + " Controlling,";
                    }
                    if (Que7.la_q7_MM == "yes")
                    {
                        Q7 = Q7 + " Materials Management,";
                    }

                    if (Que7.la_q7_SD == "yes")
                    {
                        Q7 = Q7 + " Sales & Distribution,";
                    }

                    if (Que7.la_q7_PP == "yes")
                    {
                        Q7 = Q7 + " Production Planning,";
                    }

                    if (Que7.la_q7_QM == "yes")
                    {
                        Q7 = Q7 + " Quality Management,";
                    }
                    if (Que7.la_q7_PS == "yes")
                    {
                        Q7 = " Project Systems,";
                    }

                    if (Que7.la_q7_PM == "yes")
                    {
                        Q7 = Q7 + " Plant Maintenance,";
                    }

                    if (Que7.la_q7_CS == "yes")
                    {
                        Q7 = Q7 + " Customer Service,";
                    }

                    if (Que7.la_q7_HR == "yes")
                    {
                        Q7 = Q7 + " Human Resources,";
                    }

                    if (Que7.la_q7_WM == "yes")
                    {
                        Q7 = Q7 + " Warehouse Management,";
                    }

                    if (Que7.la_q7_LO_VC == "yes")
                    {
                        Q7 = Q7 + " Variant Configuration,";
                    }

                    if (Que7.la_q7_LO_WTY == "yes")
                    {
                        Q7 = Q7 + " Warranty Management,";
                    }

                    if (Que7.la_q7_LO_SPM == "yes")
                    {
                        Q7 = Q7 + " Spare Parts Management,";
                    }

                    if (Que7.la_q7_other != "")
                    {
                        Q7 = Q7 + Que7.la_q7_other;
                    }

                    #endregion Q2
                    ss.SetParameterValue("Q7", Q7);


                    ss.SetParameterValue("Q11", Que11.la_q11_1.ToString() + " , " + Que11.la_q11_2.ToString());


                    ss.SetParameterValue("Q13", que13);

                    ss.SetParameterValue("Q14", que14);

                    ss.SetParameterValue("Q15", Que15.la_q15_1 + " , " + Que15.la_q15_2.ToString());

                    ss.SetParameterValue("f1", fq1.f1);
                    ss.SetParameterValue("f2", fq1.f2);
                    ss.SetParameterValue("f3", fq1.f3);
                    ss.SetParameterValue("f4", fq2.f4);
                    ss.SetParameterValue("f5", fq2.f5);
                    ss.SetParameterValue("f6", fq2.f6);
                    ss.SetParameterValue("f7", fq3.f7);
                    ss.SetParameterValue("f8", fq3.f8);


                    ExportOptions CrExportOptions;
                    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                    CrDiskFileDestinationOptions.DiskFileName = outputPath + User_ID + ".pdf";
                    CrExportOptions = ss.ExportOptions;
                    {
                        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                        CrExportOptions.FormatOptions = CrFormatTypeOptions;
                    }
                    ss.Export();
                    con1.Open();
                    string q = "Select Email from  users  WHERE User_ID = " + User_ID + " ";
                    MySqlCommand cmd3 = new MySqlCommand(q, con1);
                   var a= cmd3.ExecuteScalar();
                    con1.Close();
                    Boolean S = @base.AddQuestionnaire_Mail(Convert.ToInt32(User_ID), a.ToString());
                    con1.Open();
                    string myQuery = "UPDATE question SET MailStatus = " + S + " WHERE User_ID = " + User_ID + " ";
                    MySqlCommand cmd2 = new MySqlCommand(myQuery, con1);
                    cmd2.ExecuteNonQuery();
                    con1.Close();
                }
                catch (Exception ex)
                {

                    _log.createLog(ex, "-->CrystalReportPDF-->Loop" + ex.Message.ToString());
                }
                finally
                {
                    if (ss != null)
                    {
                        ss.Close();
                        ss.Dispose();
                    }
                }

            }
        }


        #region Models
        public class Question1
        {
            public string la_q1 { get; set; }
        }

        public class Question2
        {
            public string la_q2_1 { get; set; }
            public string la_q2_2 { get; set; }
            public string la_q2_3 { get; set; }
            public string la_q2_4 { get; set; }
            public string la_q2_5 { get; set; }
            public string la_q2_6 { get; set; }

        }

        public class Question3
        {
            public string la_q3 { get; set; }
        }

        public class Question4
        {

            public string la_q4_1 { get; set; }
            public string la_q4_2 { get; set; }
            public string la_q4_3 { get; set; }
            public string la_q4_4 { get; set; }

        }

        public class Question5
        {
            public string la_q5 { get; set; }
        }

        public class Question6
        {
            public string la_q6 { get; set; }
        }

        public class Question7
        {

            public string la_q7_FI { get; set; }
            public string la_q7_CO { get; set; }
            public string la_q7_MM { get; set; }
            public string la_q7_SD { get; set; }
            public string la_q7_PP { get; set; }
            public string la_q7_QM { get; set; }
            public string la_q7_PS { get; set; }
            public string la_q7_PM { get; set; }
            public string la_q7_CS { get; set; }
            public string la_q7_HR { get; set; }
            public string la_q7_WM { get; set; }
            public string la_q7_LO_VC { get; set; }
            public string la_q7_LO_WTY { get; set; }
            public string la_q7_LO_SPM { get; set; }
            public string la_q7_other { get; set; }
        }

        public class question8
        {

            public string la_q8_HCM { get; set; }
            public string la_q8_CRM { get; set; }
            public string la_q8_SRM { get; set; }
            public string la_q8_SCM { get; set; }
            public string la_q8_SNC { get; set; }
            public string la_q8_SEM { get; set; }
            public string la_q8_PLM { get; set; }
            public string la_q8_APO { get; set; }
            public string la_q8_EWM { get; set; }
            public string la_q8_TM { get; set; }
            public string la_q8_TRM { get; set; }
            public string la_q8_other { get; set; }
        }

        public class Question9
        {
            public List<string> MyArray { get; set; }

        }

        public class Question10
        {
            public string la_q10 { get; set; }
        }
        public class Question11
        {
            public string la_q11_1 { get; set; }
            public string la_q11_2 { get; set; }

        }

        public class Question12
        {
            public string la_q12 { get; set; }
        }

        public class Question13
        {
            public string la_q13 { get; set; }
        }

        public class Question14
        {
            public string la_q14 { get; set; }
        }

        public class Question15
        {
            public string la_q15_1 { get; set; }
            public string la_q15_2 { get; set; }

        }

        public class FQ1
        {
            public string f1 { get; set; }
            public string f2 { get; set; }
            public string f3 { get; set; }

        }

        public class FQ2
        {
            public string f4 { get; set; }
            public string f5 { get; set; }
            public string f6 { get; set; }

        }

        public class FQ3
        {
            public string f7 { get; set; }
            public string f8 { get; set; }

        } 

        #endregion
    }
}