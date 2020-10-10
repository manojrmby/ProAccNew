using ProACC_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ProAcc.BL
{
    public class ResetMailLink
    {
        private ProAccEntities db = new ProAccEntities();
        private readonly string userName = WebConfigurationManager.AppSettings["Mail_UserName"];
        private readonly string password = WebConfigurationManager.AppSettings["Mail_Password"];
        private readonly string serverName = WebConfigurationManager.AppSettings["Mail_Client"];
        readonly int port = Convert.ToInt32(WebConfigurationManager.AppSettings["Mail_Port"]);
        Base _base = new Base();
        public async Task FetchLink(string emailId,string msg,string subject)
        {
            bool isSend;
            try
            {
                bool result = await Task.Run(()=>SendMail(emailId, msg, subject));
                if (result == true)
                {
                    isSend = true;
                }

                else
                {
                   isSend=false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            isSend=true;
        }
        private bool SendMail(string emailId, string msg, string subject)
        {
            bool isSend = false;
            try
            {
                var _userEmail = (from emp in db.UserMasters
                               where emp.EMail == emailId && emp.isActive == true
                               select emp.EMail).FirstOrDefault();

                if (_userEmail != null)
                {
                    var body = msg;
                    var message = new MailMessage();

                    message.To.Add(new MailAddress("manogekumar@promantus.com"));
                    message.From = new MailAddress(_base.Decrypt(userName));
                    message.Subject = !string.IsNullOrEmpty(subject) ? subject : "This is for Demo Purpose";
                    message.Body = body;
                    message.IsBodyHtml = true;
                    message.Priority = MailPriority.High;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = _base.Decrypt(userName),
                            Password = _base.Decrypt(password)
                        };

                        smtp.Credentials = credential;
                        smtp.Host = serverName;
                        smtp.Port = port;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }
                    isSend = true;
                    message.Dispose();
                    
                }
                else
                {
                    return isSend;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return isSend;
        }
    }

}