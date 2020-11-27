using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Model.MailTemplates
{
    public class MailVerificationTemplate : MailTemplate
    {
        public MailVerificationTemplate(MailAddress mailAddress) : base(mailAddress)
        {
            base.Subject = "Mail verification";
            base.Title = "HELLO";
            base.Text = "This is verifiy";
            base.BuildBody();
        }
    }
}
