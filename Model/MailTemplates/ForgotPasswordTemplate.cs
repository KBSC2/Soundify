using System.Net.Mail;

namespace Model.MailTemplates
{
    public class ForgotPasswordTemplate : MailTemplate
    {
        public ForgotPasswordTemplate(MailAddress mailAddress, string token) : base(mailAddress)
        {
            base.Subject = "Forgot your password";
            base.Title = "Dear User";
            base.Text = $"We got a request to reset you password" +
                        $"<br> if you wish to change your password, please copy the token below and follow the instructions." +
                        $"<br> {token}" +
                        $"<br> " + 
                        $"<br> if you didn't mean to reset your password, just ignore this email; No changes will be made.";
            base.BuildBody();
        }
    }
}
