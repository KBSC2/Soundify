using System.Net.Mail;

namespace Model.MailTemplates
{
    public class MailVerificationTemplate : MailTemplate
    {
        public MailVerificationTemplate(MailAddress mailAddress, string token) : base(mailAddress)
        {
            base.Subject = "Email verification";
            base.Title = "Please verify your email";
            base.Text = "<p style=\"color:#fff; font-size:15px;line-height:24px; margin:0;\">" +
                        "   <br> Please verify your email by copying the following code and entering it into the Soundify application" +
                        "   <br> " +
                        "</p>" +
                        "<h1 style=\"margin-top:35px; color:#fff; padding:10px 24px;display:inline-block;\">" +
                        $"   {token}" +
                        "</h1>";
            base.BuildBody();
        }
    }
}
