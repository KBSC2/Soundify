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
            Body = $"<h1> {Title} </h1>" +
                   $"<p> {Text} </p>";
        }

    }
}
