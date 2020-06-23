using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ProAcc.BL
{
    public class Mail
    {
        public Boolean SendEmail()
        {
            Boolean Status = false;

            try
            {
                string Client = ConfigurationManager.AppSettings["Mail_Client"].ToString();
                string UserName = ConfigurationManager.AppSettings["Mail_UserName"].ToString();
                string Password = ConfigurationManager.AppSettings["Mail_Password"].ToString();
                int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Mail_Port"].ToString());
                Boolean SSL = Convert.ToBoolean(ConfigurationManager.AppSettings["Mail_SSL"].ToString());


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Client);

                mail.From = new MailAddress(UserName);
                mail.To.Add(UserName);
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";
                mail.IsBodyHtml = true;
                SmtpServer.Port = Port;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(UserName, Password);
                SmtpServer.EnableSsl = SSL;

                SmtpServer.Send(mail);
                Status = true;
            }
            catch (Exception ex)
            {
                //Log
            }
            return Status;
        }
    }
}