using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Model.MailTemplates
{
    public class MailSongApprovalTemplate : MailTemplate
    {
        public MailSongApprovalTemplate(MailAddress mailAddress, string artist, string songName) : base(mailAddress)
        {
            base.Subject = "Song Approval";
            base.Title = $"{artist} would like to upload a song: '{songName}'";
            base.Text = $"{artist} would like to upload a song: '{songName}'. " +
                        "For more information, please login to the application and approve or decline the request.";
            base.BuildBody();
        }
    }
}
