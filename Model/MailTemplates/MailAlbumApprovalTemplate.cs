using System.Net.Mail;

namespace Model.MailTemplates
{
    public class MailAlbumApprovalTemplate : MailTemplate
    {
        public MailAlbumApprovalTemplate(MailAddress mailAddress, string artist, string songName) : base(mailAddress)
        {
            base.Subject = "Song Approval";
            base.Title = $"{artist} would like to upload a album: <b>'{songName}'</b>";
            base.Text = "<p style=\"color:#fff; font-size:15px;line-height:24px; margin:0;\">" +
                       $"   {artist} would like to upload a album: <b>'{songName}'</b>. " +
                        "   <br> " +
                        "   <br style=\"color:#fff;\">Please login to Soundify to approve or decline this request" +
                        "</p>";
            base.BuildBody();
        }
    }
}
