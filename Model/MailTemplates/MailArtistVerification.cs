using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Model.MailTemplates
{
    public class MailArtistVerification : MailTemplate
    {
        public MailArtistVerification(MailAddress mailAddress, string artistName, string artistReason) : base(mailAddress)
        {
            base.Subject = $"{artistName} would like you to verify";
            base.Title = "Dear Admin";
            base.Text = $"The user {artistName} would like to verify with the following reason:" +
                        $"<br>'{artistReason}'";
            base.BuildBody();
        }
    }
}
