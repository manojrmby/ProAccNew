using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
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
        LogHelper _log = new LogHelper();
        public void Report()
        {

            try
            {
                SqlConnection con1 = new SqlConnection();
                con1.ConnectionString = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
                SqlDataAdapter da = new SqlDataAdapter("Select top 1 * from Question order by id desc", con1);
                DataTable dt = new DataTable();
                da.Fill(dt);
                Base @base = new Base();


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
                foreach (DataRow dr in dt.Rows)
                {

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

                        //String FileName = User_ID + ".pdf";

                        CrystalReport crypt = new CrystalReport();

                        //crypt.SetParameterValue("CompanyNam", "MyCompany");
                        //crypt.SetParameterValue("Q1", que1);

                        //crypt.SetParameterValue("Q2.1", Que2.la_q2_1);
                        //crypt.SetParameterValue("Q2.2", Que2.la_q2_2);
                        //crypt.SetParameterValue("Q2.3", Que2.la_q2_3);
                        //crypt.SetParameterValue("Q2.4", Que2.la_q2_4);
                        //crypt.SetParameterValue("Q.2.5", Que2.la_q2_5);
                        //crypt.SetParameterValue("Q2.6", Que2.la_q2_6);

                        //crypt.SetParameterValue("Q3", que3);

                        crypt.SetParameterValue("Q4.1", Que4.la_q4_1);
                        crypt.SetParameterValue("Q4.2", Que4.la_q4_2);
                        crypt.SetParameterValue("Q4.3", Que4.la_q4_3);
                        //crypt.SetParameterValue("Q4.4", Que4.la_q4_4);

                        //crypt.SetParameterValue("Q5", que5);
                        crypt.SetParameterValue("Q6", que6);

                        //crypt.SetParameterValue("Q7.1", Que7.la_q7_FI);
                        //crypt.SetParameterValue("Q7.2", Que7.la_q7_CO);
                        //crypt.SetParameterValue("Q7.3", Que7.la_q7_MM);
                        //crypt.SetParameterValue("Q7.4", Que7.la_q7_SD);
                        //crypt.SetParameterValue("Q7.5", Que7.la_q7_PP);
                        //crypt.SetParameterValue("Q7.6", Que7.la_q7_QM);
                        //crypt.SetParameterValue("Q7.7", Que7.la_q7_PS);
                        //crypt.SetParameterValue("Q7.8", Que7.la_q7_PM);
                        //crypt.SetParameterValue("Q7.9", Que7.la_q7_CS);
                        //crypt.SetParameterValue("Q7.10", Que7.la_q7_HR);
                        //crypt.SetParameterValue("Q7.11", Que7.la_q7_WM);
                        //crypt.SetParameterValue("Q7.12", Que7.la_q7_LO_VC);
                        //crypt.SetParameterValue("Q7.13", Que7.la_q7_LO_WTY);
                        //crypt.SetParameterValue("Q7.14", Que7.la_q7_LO_SPM);
                        //crypt.SetParameterValue("Q7.15", Que7.la_q7_other);


                        //crypt.SetParameterValue("Q8.1", Que8.la_q8_HCM);
                        //crypt.SetParameterValue("Q8.2", Que8.la_q8_CRM);
                        //crypt.SetParameterValue("Q8.3", Que8.la_q8_SRM);
                        //crypt.SetParameterValue("Q8.4", Que8.la_q8_SCM);
                        //crypt.SetParameterValue("Q8.5", Que8.la_q8_SNC);
                        //crypt.SetParameterValue("Q8.6", Que8.la_q8_SEM);
                        //crypt.SetParameterValue("Q8.7", Que8.la_q8_PLM);
                        //crypt.SetParameterValue("Q8.8", Que8.la_q8_APO);
                        //crypt.SetParameterValue("Q8.9", Que8.la_q8_EWM);
                        //crypt.SetParameterValue("Q8.10", Que8.la_q8_TM);
                        //crypt.SetParameterValue("Q8.11", Que8.la_q8_TRM);
                        //crypt.SetParameterValue("Q8.12", Que8.la_q8_other);

                        //crypt.SetParameterValue("Q9", Que9[0].ToString() + " " + Que9[1].ToString());

                        //crypt.SetParameterValue("Q10", que10);

                        crypt.SetParameterValue("Q11", Que11.la_q11_1.ToString() + " , " + Que11.la_q11_2.ToString());

                        //crypt.SetParameterValue("Q12", que12);

                        crypt.SetParameterValue("Q13", que13);

                        crypt.SetParameterValue("Q14", que14);

                        crypt.SetParameterValue("Q15", Que15.la_q15_1 + " , " + Que15.la_q15_2.ToString());

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

                        //ss.Load(@"D:\Office\Projects\ProACC\ProAccNew_23_07_2020 - Copy\ProAccNew\ProAcc\BL\Model\CrystalReport.rpt");
                        ss.Load(Rpt_Path);
                        //ss.SetParameterValue("CompanyNam", "MyCompany");
                        //ss.SetParameterValue("Q1", que1);

                        //ss.SetParameterValue("Q2.1", Que2.la_q2_1);
                        //ss.SetParameterValue("Q2.2", Que2.la_q2_2);
                        //ss.SetParameterValue("Q2.3", Que2.la_q2_3);
                        //ss.SetParameterValue("Q2.4", Que2.la_q2_4);
                        //ss.SetParameterValue("Q.2.5", Que2.la_q2_5);
                        //ss.SetParameterValue("Q2.6", Que2.la_q2_6);

                        #region Q2
                        String Q2 = "";
                        if (Que2.la_q2_1 == "yes")
                        {
                            Q2 = Q2 + "Yes";
                        }
                        else
                        {
                            Q2 = Q2 + "NO";
                        }
                        if (Que2.la_q2_2 == "yes")
                        {
                            Q2 = Q2 + ", Yes";
                        }
                        else
                        {
                            Q2 = Q2 + ", NO";
                        }
                        if (Que2.la_q2_3 == "yes")
                        {
                            Q2 = Q2 + ", Yes";
                        }
                        else
                        {
                            Q2 = Q2 + ", NO";
                        }
                        if (Que2.la_q2_4 == "yes")
                        {
                            Q2 = Q2 + ", Yes";
                        }
                        else
                        {
                            Q2 = Q2 + ", NO";
                        }
                        if (Que2.la_q2_5 == "yes")
                        {
                            Q2 = Q2 + ", Yes";
                        }
                        else
                        {
                            Q2 = Q2 + ", NO";
                        }
                        if (Que2.la_q2_6 == "yes")
                        {
                            Q2 = Q2 + ", Yes";
                        }
                        else
                        {
                            Q2 = Q2 + ", NO";
                        }
                        #endregion Q2
                        ss.SetParameterValue("Q2", Q2);

                        //ss.SetParameterValue("Q3", que3);

                        ss.SetParameterValue("Q4.1", Que4.la_q4_1);
                        ss.SetParameterValue("Q4.2", Que4.la_q4_2);
                        ss.SetParameterValue("Q4.3", Que4.la_q4_3);
                        //ss.SetParameterValue("Q4.4", Que4.la_q4_4);

                        //ss.SetParameterValue("Q5", que5);
                        ss.SetParameterValue("Q6", que6);


                        #region Q7
                        String Q7 = "";
                        if (Que7.la_q7_FI == "yes")
                        {
                            Q7 = Q7 + "Yes";
                        }
                        else
                        {
                            Q7 = Q7 + "NO";
                        }
                        if (Que7.la_q7_CO == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_MM == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_SD == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_PP == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_QM == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_PS == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_PM == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_CS == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_HR == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_WM == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_LO_VC == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_LO_WTY == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ",NO";
                        }
                        if (Que7.la_q7_LO_SPM == "yes")
                        {
                            Q7 = Q7 + ", Yes";
                        }
                        else
                        {
                            Q7 = Q7 + ", NO";
                        }
                        //if (Que7.la_q7_other == "yes")
                        //{
                        //    Q7 = Q7 + "Yes";
                        //}
                        //else
                        //{
                        //     Q7 = Q7 + ",NO";
                        //}

                        #endregion Q2
                        ss.SetParameterValue("Q7", Q7);

                        //ss.SetParameterValue("Q7.1", Que7.la_q7_FI);
                        //ss.SetParameterValue("Q7.2", Que7.la_q7_CO);
                        //ss.SetParameterValue("Q7.3", Que7.la_q7_MM);
                        //ss.SetParameterValue("Q7.4", Que7.la_q7_SD);
                        //ss.SetParameterValue("Q7.5", Que7.la_q7_PP);
                        //ss.SetParameterValue("Q7.6", Que7.la_q7_QM);
                        //ss.SetParameterValue("Q7.7", Que7.la_q7_PS);
                        //ss.SetParameterValue("Q7.8", Que7.la_q7_PM);
                        //ss.SetParameterValue("Q7.9", Que7.la_q7_CS);
                        //ss.SetParameterValue("Q7.10", Que7.la_q7_HR);
                        //ss.SetParameterValue("Q7.11", Que7.la_q7_WM);
                        //ss.SetParameterValue("Q7.12", Que7.la_q7_LO_VC);
                        //ss.SetParameterValue("Q7.13", Que7.la_q7_LO_WTY);
                        //ss.SetParameterValue("Q7.14", Que7.la_q7_LO_SPM);
                        //ss.SetParameterValue("Q7.15", Que7.la_q7_other);


                        //ss.SetParameterValue("Q8.1", Que8.la_q8_HCM);
                        //ss.SetParameterValue("Q8.2", Que8.la_q8_CRM);
                        //ss.SetParameterValue("Q8.3", Que8.la_q8_SRM);
                        //ss.SetParameterValue("Q8.4", Que8.la_q8_SCM);
                        //ss.SetParameterValue("Q8.5", Que8.la_q8_SNC);
                        //ss.SetParameterValue("Q8.6", Que8.la_q8_SEM);
                        //ss.SetParameterValue("Q8.7", Que8.la_q8_PLM);
                        //ss.SetParameterValue("Q8.8", Que8.la_q8_APO);
                        //ss.SetParameterValue("Q8.9", Que8.la_q8_EWM);
                        //ss.SetParameterValue("Q8.10", Que8.la_q8_TM);
                        //ss.SetParameterValue("Q8.11", Que8.la_q8_TRM);
                        //ss.SetParameterValue("Q8.12", Que8.la_q8_other);

                        //ss.SetParameterValue("Q9", Que9[0].ToString() + " " + Que9[1].ToString());

                        //ss.SetParameterValue("Q10", que10);

                        ss.SetParameterValue("Q11", Que11.la_q11_1.ToString() + " , " + Que11.la_q11_2.ToString());

                        //ss.SetParameterValue("Q12", que12);

                        ss.SetParameterValue("Q13", que13);

                        ss.SetParameterValue("Q14", que14);

                        ss.SetParameterValue("Q15", Que15.la_q15_1 + " , " + Que15.la_q15_2.ToString());

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
                        Boolean S = @base.AddQuestionnaire_Mail(Convert.ToInt32(User_ID), "");//Add Mail Id

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
            catch (Exception ex)
            {

                _log.createLog(ex, "-->CrystalReportPDF" + ex.Message.ToString());
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