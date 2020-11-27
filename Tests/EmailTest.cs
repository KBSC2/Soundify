using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Controller;
using Model.MailTemplates;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EmailTest
    {
        EmailController<MailVerificationTemplate> _emailVerificationController = new EmailController<MailVerificationTemplate>();

        [Test]
        public void Controller_EmailController_SendSimpleMail()
        {
            var mail = new MailVerificationTemplate(new MailAddress("sander__pol@outlook.com"));
            _emailVerificationController.SendEmail(mail);
        }
    }
}
