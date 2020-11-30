using System.Net.Mail;

namespace Model.MailTemplates
{
    public class MailVerificationTemplate : MailTemplate
    {
        public MailVerificationTemplate(MailAddress mailAddress, string token) : base(mailAddress)
        {
            base.Subject = "Email verification";
            base.Title = "Email verification";
            base.Text = "Please verify you're email by copying the following code " +
                        "and entering it into the Soundify application" +
                        $"<h1>{token}</h1>";
            base.BuildBody();
        }
    }
}
