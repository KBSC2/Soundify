using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Controller
{
    public class EmailController
    {
        public SmtpClient smtpClient { get;} 

        public EmailController()
        {
            var conf = GetSMTPGMAILConfiguration();
            smtpClient = new SmtpClient(conf.GetValueOrDefault("Host"))
            {
                Port = 587,
                Credentials = new NetworkCredential(conf.GetValueOrDefault("Email"), conf.GetValueOrDefault("Password")),
                EnableSsl = true

            };

        }

        public static Dictionary<string, string> GetSMTPGMAILConfiguration()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(@"View.dll");
            foreach (var s in configuration.ConnectionStrings.ConnectionStrings["SMTPGMAIL"].ConnectionString.Split(";"))
            {
                string[] split = s.Trim().Split("=");
                if (split.Length == 2)
                    result.Add(split[0].Trim(), split[1].Trim());
            }

            return result;
        }
        public void SendEmail()
        {
            var mailMessage = new MailMessage()
            {
                From = new MailAddress("info.soundify@gmail.com"),
                Subject = "Test",
                Body = "<h1>HELLO</H1>",
                IsBodyHtml = true
            };
            mailMessage.To.Add("sander__pol@outlook.com");
            smtpClient.Send(mailMessage);
        }
    }
}
