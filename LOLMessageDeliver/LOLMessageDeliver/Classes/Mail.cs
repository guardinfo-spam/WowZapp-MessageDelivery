//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Net.Mail;
//using System.Web.Configuration;
//using System.Configuration;
//using System.Net.Configuration;

//namespace LOLMessageDelivery
//{
//    public static class Mail
//    {

//        public static bool SendMail(string toAddress, string subject, string body)
//        {

//            try
//            {
                
//                Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration("~\\Web.config");

//                MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;

//                int port = 25;
//                string host = "";
//                string password = "";
//                string username = "";

//                if (mailSettings != null)
//                {

//                port = mailSettings.Smtp.Network.Port;
//                host = mailSettings.Smtp.Network.Host;
//                password = mailSettings.Smtp.Network.Password;
//                username = mailSettings.Smtp.Network.UserName;

//                }
//                MailMessage tmpMsg = new MailMessage();

//                tmpMsg.To.Add(new MailAddress(toAddress));
//                tmpMsg.From = new MailAddress(username);
//                tmpMsg.Subject = subject;
//                tmpMsg.Body = body;

//                tmpMsg.IsBodyHtml = true;

//                var smtpSend = new SmtpClient();

//                smtpSend.Send(tmpMsg);
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//    }
//}