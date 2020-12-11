using System.Net.Mail;
using System.Net.Mime;

namespace Model.MailTemplates
{
    public class ForgotPasswordTemplate : MailTemplate
    {
        public ForgotPasswordTemplate(MailAddress mailAddress, string token) : base(mailAddress)
        {
            base.Subject = "Forgot your password";
            base.Title = "We got a request to reset your password";

            base.Text = "<p style=\"color:#fff; font-size:15px;line-height:24px; margin:0;\">" +
                        "   <br> if you wish to change your password, please copy the token below and follow the instructions." +
                        "   <br> " +
                        "   <br> if you didn't mean to reset your password, just ignore this email; No changes will be made." +
                        "</p>" +
                        "<h1 style=\"margin-top:35px; color:#fff;padding:10px 24px;display:inline-block;\">" +
                       $"   {token}" +
                        "</h1>";

            base.BuildBody();
        }
    }
}
