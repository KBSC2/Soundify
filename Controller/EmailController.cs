using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Model.MailTemplates;

namespace Controller
{
    public class EmailController
    {
        public SmtpClient SmtpClient { get;} 


        public EmailController()
        {
            var conf = GetSMTPGMAILConfiguration();
            SmtpClient = new SmtpClient(conf.GetValueOrDefault("Host"))
            {
                Port = 587,
                Credentials = new NetworkCredential(conf.GetValueOrDefault("Email"), conf.GetValueOrDefault("Password")),
                EnableSsl = true
            };
        }

        /**
         * gets variables for connection with the mailserver
         *
         * @return Dictionary<string, string> : configuration for the email client
         */
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

        /**
         * sends the email
         *
         * @param mailTemplate and mailAddress gets the template variant and the address where the mail needs to go
         *
         * @return void
         */
        public void SendEmail<T>(T mailTemplate, string toMailAddress) where T : MailTemplate
        {
            var mailMessage = new MailMessage()
            {
                From = mailTemplate.MailAddress,
                Subject = mailTemplate.Subject,
                Body = mailTemplate.Body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toMailAddress);
            SmtpClient.Send(mailMessage);
        }
    }
}
