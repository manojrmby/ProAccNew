using ProAcc.BL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using ProACC_DB;

namespace ProAcc.BL
{
    public class Mail
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        GetQuestionary GQ = new GetQuestionary();
        CrystalReportPDF pDF = new CrystalReportPDF();
        private readonly string _TemplatePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Mail_FolderPath"].ToString());
        private readonly string OutputPath_pdf = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["OutputPath_pdf"].ToString());
        private readonly string _Mail_CheckInterval = WebConfigurationManager.AppSettings["Mail_CheckInterval"];
        private readonly string _MailRunStatus = WebConfigurationManager.AppSettings["MailRunStatus"];
        private readonly string _Mail_EnableTest = ConfigurationManager.AppSettings["Mail_EnableTest"].ToString();
        private readonly string _Mail_TestToID = ConfigurationManager.AppSettings["Mail_TestToID"].ToString();
        private readonly string serverName = WebConfigurationManager.AppSettings["Mail_Client"];
        private readonly string userName = WebConfigurationManager.AppSettings["Mail_UserName"];
        private readonly string password = WebConfigurationManager.AppSettings["Mail_Password"];
        readonly MailAddress fromAddress = new MailAddress(WebConfigurationManager.AppSettings["Mail_UserName"]);
        readonly int port = Convert.ToInt32(WebConfigurationManager.AppSettings["Mail_Port"]);
        

        internal void StartMailSend()
        {
            Boolean MailRunstatus = Convert.ToBoolean(_MailRunStatus);
            if (MailRunstatus)
            {
                _ = StartTimer();
            }
            //GQ.StartGQPULL();
        }
        public async Task StartTimer()
        {
          int T= Convert.ToInt32(_Mail_CheckInterval);
            await Task.Run(async () =>
            {
                while (true)
                {
                    _Log.createLog("Mail Started");
                    _ = SendAsyncMail();
                    pDF.Report();
                    await Task.Delay(T);
                    Thread.Sleep(100000);
                    //, cancellationToken);
                    //if (cancellationToken.IsCancellationRequested)
                    //break;
                }
            });

        }
        public async Task SendAsyncMail()
        {
            try
            
            {
                Boolean priority = false,MailTestEnable=true;
                string TestToId = "";
                List<MailModel> MailS = _Base.GetMailList();
                MailTestEnable = Convert.ToBoolean(_Mail_EnableTest);
                TestToId = _Mail_TestToID;

                foreach (var item in MailS)
                {
                    Send_Mail SM = new Send_Mail();

                    SM.subject = item.Subject;
                    SM.body = PopulateBody(item.TemplateName.Trim(),item.Body,item.Name);
                    SM.priority = priority;
                    SM.ID = item.Running_ID;
                    SM.Q_UserID = item.Q_UserID;

                    string To="",Name="";
                    if (MailTestEnable)
                    {
                        To = TestToId;
                        Name = "Test";

                    }
                    else
                    {
                        To = item.To.ToString();
                        Name = item.Name.ToString();
                    }
                    MailAddress toAddress = new MailAddress(To,Name);
                    await Task.Run(() => this.Send(toAddress, SM));
                }
                _Log.createLog("All Mail Sent");
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->SendAsyncMail" + ex.Message.ToString());
            }
            
            
        }
        
        public void Send(MailAddress toAddress, Send_Mail SM)
        {
            Task.Factory.StartNew(() => SendEmail(toAddress, SM), TaskCreationOptions.LongRunning);
        }

        private void SendEmail(MailAddress toAddress, Send_Mail SM)
        {
            try
            {

                var message = new MailMessage(fromAddress, toAddress);

                message.Subject = SM.subject;
                message.Body = SM.body;
                message.IsBodyHtml = true;
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                if (SM.priority) message.Priority = MailPriority.High;
                if (SM.Q_UserID >0)
                {
                    var fs = new FileStream(OutputPath_pdf + SM.Q_UserID + ".pdf", FileMode.Open);
                    message.Attachments.Add(new System.Net.Mail.Attachment(fs, "Questionnaire.pdf", "application/pdf"));
                }
                Thread.Sleep(1000);

                SmtpClient client = new SmtpClient(serverName, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                //Convert.ToBoolean(WebConfigurationManager.AppSettings["SmtpSsl"]);
                client.UseDefaultCredentials = false;

                NetworkCredential smtpUserInfo = new NetworkCredential(userName, password);
                client.Credentials = smtpUserInfo;
                ProAccEntities db = new ProAccEntities();
                client.Credentials = smtpUserInfo;

                


                var mm = db.MailMasters.Where(x => x.Running_ID == SM.ID).FirstOrDefault().MailStatus;
                                                                                                   
                if (mm == false)
                {
                    client.Send(message);
                    _Log.createLog(SM.ID + "--->" + toAddress.ToString());
                    _Base.UpdateMailList(SM.ID);
                }
                client.Dispose();
                message.Dispose();

            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->SendEmail" + ex.Message.ToString());
            }

        }
        private string PopulateBody(string TemplateName,string Body,string Name)
        {
            string body = string.Empty;
            try
            {
                string pattern = "(,)";
                string input = Body;
                string Task = "", Phase = "", projectName = "", Instance = "",User_ID="";
                Regex regex = new Regex(pattern);

                using (StreamReader reader = new StreamReader(_TemplatePath + TemplateName.Trim() + ".html"))
                {
                    body = reader.ReadToEnd();
                }

                if (TemplateName.Trim()== "T5")
                {
                    //string[] substrings = regex.Split(input);
                    //User_ID= substrings[0].ToString();
                }
                else
                {
                    string[] substrings = regex.Split(input);
                    Task = substrings[0].ToString();
                    Phase = substrings[2].ToString();
                    Instance = substrings[4].ToString();
                    projectName = substrings[6].ToString();

                    body = body.Replace("{ManagerName}", Name);
                    body = body.Replace("{TaskName}", Task);
                    body = body.Replace("{PhaseName}", Phase);
                    body = body.Replace("{projectName}", projectName);
                    body = body.Replace("{InstanceName}", Instance);

                }
               
                //body = body.Replace("{Description}", description);
            }
            catch (Exception ex)
            {
                _Log.createLog(ex, "-->PopulateBody" + ex.Message.ToString());
            }


            return body;
        }

        public class Send_Mail
        {
            public string subject { get; set; }
            public string body { get; set; }
            public Boolean priority { get; set; }

            public int ID { get; set; }
            public int Q_UserID { get; set; }


        }


        //private void SendEmail(String ToMailId)
        //{
        //    Boolean Status = false;

        //    try
        //    {
        //        string Client = ConfigurationManager.AppSettings["Mail_Client"].ToString();
        //        string UserName = ConfigurationManager.AppSettings["Mail_UserName"].ToString();
        //        string Password = ConfigurationManager.AppSettings["Mail_Password"].ToString();
        //        int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Port"].ToString());
        //        Boolean SSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_SSL"].ToString());


        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient(Client);

        //        mail.From = new MailAddress(UserName);
        //        mail.To.Add(ToMailId);
        //        mail.Subject = "Test Mail";
        //        mail.Body = "This is for testing SMTP mail from GMAIL";
        //        mail.IsBodyHtml = true;
        //        SmtpServer.Port = Port;
        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
        //        SmtpServer.EnableSsl = SSL;

        //        SmtpServer.Send(mail);
        //        Status = true;
        //        SmtpServer.Dispose();
        //        mail.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log
        //    }
        //    //return Status;
        //}
    }
}