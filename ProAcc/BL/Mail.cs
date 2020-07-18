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

namespace ProAcc.BL
{
    public class Mail
    {
        Base _Base = new Base();
        LogHelper _Log = new LogHelper();
        GetQuestionary GQ = new GetQuestionary();
        private string _TemplatePath = System.Web.Hosting.HostingEnvironment.MapPath(ConfigurationManager.AppSettings["Mail_FolderPath"].ToString());
        internal void StartMailSend()
        {
            StartTimer();
            //GQ.StartGQPULL();
        }
        public async Task StartTimer()
        {
          int T= Convert.ToInt32( WebConfigurationManager.AppSettings["Mail_CheckInterval"]);
            //TimeSpan ts = TimeSpan.FromSeconds(T);

            await Task.Run(async () =>
            {
                while (true)
                {
                    _Log.createLog("Mail Started");
                    _ = SendAsyncMail();
                    await Task.Delay(T);
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
                string subject, body,Template;
                Boolean priority = false,MailTestEnable=true;
                string TestToId = "";
                subject = "";
                body = "";
                Template = "";
                List<MailModel> MailS = _Base.GetMailList();
                MailTestEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_EnableTest"].ToString());
                TestToId = ConfigurationManager.AppSettings["Mail_TestToID"].ToString();

                foreach (var item in MailS)
                {
                    subject = item.Subject;
                    body = PopulateBody(item.TemplateName,item.Body,item.Name);

                    string To="",Name="";
                    if (MailTestEnable)
                    {
                        To = TestToId;
                        Name = "Test";

                    }
                    else
                    {
                        //To = item.To.ToString();
                        Name = item.Name.ToString();
                    }
                    MailAddress toAddress = new MailAddress(To,Name);
                    await Task.Run(() => this.Send(toAddress, subject, body, priority,item.Running_ID));
                }
                _Log.createLog("All Mail Sent");
            }
            catch (Exception EX)
            {
                _Log.createLog("Mail Started -->SendAsyncMail" + EX);
                throw;
            }
            
            
        }
        
        public void Send(MailAddress toAddress, string subject, string body, bool priority,int ID)
        {
            Task.Factory.StartNew(() => SendEmail(toAddress, subject, body, priority,ID), TaskCreationOptions.LongRunning);
        }

        private void SendEmail(MailAddress toAddress, string subject, string body, bool priority,int ID)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(WebConfigurationManager.AppSettings["Mail_UserName"]);
                string serverName = WebConfigurationManager.AppSettings["Mail_Client"];
                int port = Convert.ToInt32(WebConfigurationManager.AppSettings["Mail_Port"]);
                string userName = WebConfigurationManager.AppSettings["Mail_UserName"];
                string password = WebConfigurationManager.AppSettings["Mail_Password"];

                var message = new MailMessage(fromAddress, toAddress);

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.BodyEncoding = Encoding.UTF8;
                if (priority) message.Priority = MailPriority.High;

                Thread.Sleep(1000);

                SmtpClient client = new SmtpClient(serverName, port);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                //Convert.ToBoolean(WebConfigurationManager.AppSettings["SmtpSsl"]);
                client.UseDefaultCredentials = false;

                NetworkCredential smtpUserInfo = new NetworkCredential(userName, password);
                client.Credentials = smtpUserInfo;

                client.Send(message);
                _Log.createLog(ID + "--->" + toAddress.ToString());
                _Base.UpdateMailList(ID);
                client.Dispose();
                message.Dispose();
            }
            catch (Exception Ex)
            {

                _Log.createLog("Mail Errorr --->" + Ex.ToString());
                //throw;
            }
            
        }


        //private string PopulateBody(string userName, string title, string url, string description, string TemplateName)
        private string PopulateBody(string TemplateName,string Body,string Name)
        {
            string body = string.Empty;
            try
            {

                string pattern = "(,)";
                string input = Body;
                string Task="",Phase="",projectName="",Instance="";

                // Split on hyphens from 15th character on
                Regex regex = new Regex(pattern);
                // Split on hyphens from 15th character on
                string[] substrings = regex.Split(input);
                Task = substrings[0].ToString();
                Phase = substrings[2].ToString();
                Instance = substrings[4].ToString();
                projectName = substrings[6].ToString();

                
                using (StreamReader reader = new StreamReader(_TemplatePath+ TemplateName.Trim()+".html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{ManagerName}", Name);
                body = body.Replace("{TaskName}", Task);
                body = body.Replace("{PhaseName}", Phase);
                body = body.Replace("{projectName}", projectName);
                body = body.Replace("{InstanceName}", Instance);
                //body = body.Replace("{Description}", description);
            }
            catch (Exception Ex)
            {

                throw;
            }
            
            return body;
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