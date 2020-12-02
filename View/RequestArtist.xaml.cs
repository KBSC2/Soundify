using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Controller;
using Model.MailTemplates;

namespace View
{
    /// <summary>
    /// Interaction logic for RequestArtist.xaml
    /// </summary>
    public partial class RequestArtist : Window
    {
        public RequestArtist()
        {
            InitializeComponent();
        }
        private void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var ArtistName = this.ArtistName.Text;
            var ArtistReason = this.ArtistReason.Text;

            var emailController = new EmailController<MailArtistVerification>();
            var email =  new MailArtistVerification(new MailAddress("info.soundify@gmail.com"), ArtistName, ArtistReason);
            emailController.SendEmail(email, "info.soundify@gmail.com");
            this.Close();
        }
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
