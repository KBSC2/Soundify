using System.Net.Mail;

namespace Model.MailTemplates
{
    public class MailArtistVerification : MailTemplate
    {
        public MailArtistVerification(MailAddress mailAddress, string artistName, string artistReason) : base(mailAddress)
        {
            base.Subject = $"{artistName} would like you to verify";
            base.Title = $"{artistName} would like to become an artist";
            base.Text = "<p style=\"color:#fff; font-size:15px;line-height:24px; margin:0;\">" +
                       $"   <br> The user {artistName} would like to verify with the following reason:" +
                        "   <br> " +
                       $"   <br>'{artistReason}'" +
                        "   <br> Please login to Soundify to approve or decline this request" +
                        "</p>";
            base.BuildBody();
        }
    }
}
