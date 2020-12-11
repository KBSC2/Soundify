using System.Net.Mail;

namespace Model.MailTemplates
{
    public class MailTemplate
    {
        public MailAddress MailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public MailTemplate(MailAddress mailAddress)
        {
            MailAddress = mailAddress;
        }

        public void BuildBody()
        {
            Body = "<html>" +
                   "    <body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">" +
                   "        <table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">" +
                   "            <tbody>" +
                   "                <tr>" +
                   "                    <td style=\"height:30px;\">&nbsp;</td>" +
                   "                </tr>" +
                   "                <tr>" +
                   "                    <td>" +
                   "                        <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:670px;background:#09122e; color:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">" +
                   "                            <tbody>" +
                   "                                <tr>" +
                   "                                    <td style=\"height:40px;\">&nbsp;</td>" +
                   "                                </tr>" +
                   "                                <tr>" +
                   "                                    <td style=\"text-align:center; padding-right:10px\" width=\"50%\">" +
                   "                                        <img width=\"60\" src=\"https://i.imgur.com/ZJiLFI9.png\" title=\"logo\" alt=\"logo\">" +
                   "                                        <span style=\"color:#f78D0e; font-family:'Segoe-UI',sans-serif; font-size:55px; vertical-align: top\"><b>Soundify</b></span>" +
                   "                                    </td>" +
                   "                                </tr>" +
                   "                                <tr>" +
                   "                                    <td style=\"height:40px;\">&nbsp;</td>" +
                   "                                </tr>" +
                   "                                <tr>" +
                   "                                    <td style=\"padding:0 35px;\" colspan=\"2\">" +
                   "                                        <h1 style=\"color:#fff; font-weight:500; margin:0;font-size:32px;font-family:'Segoe-UI',sans-serif;\">" +
                  $"                                            {Title}" +
                   "                                        </h1>" +
                   "                                        <span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>" +
                  $"                                        {Text}"+
                   "                                    </td>                                " +
                   "                                </tr>" +
                   "                                <tr>" +
                   "                                    <td style=\"height:40px;\">&nbsp;</td>" +
                   "                                </tr>" +
                   "                            </tbody>" +
                   "                        </table>" +
                   "                    </td>" +
                   "                </tr>" +
                   "            </tbody>" +
                   "        </table>" +
                   "    </body>" +
                   "</html>";

        }

    }
}
