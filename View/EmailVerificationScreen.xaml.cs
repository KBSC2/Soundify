using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.MailTemplates;
using Soundify;

namespace View
{
    /// <summary>
    /// Interaction logic for EmailVerificationScreen.xaml
    /// </summary>
    public partial class EmailVerificationScreen : Window
    {
        private string _previousToken { get; set; }
        private string _email { get; set; }

        public EmailVerificationScreen(string token, string email)
        {
            InitializeComponent();
            _previousToken = token;
            _email = email;
            var mailVerification = new MailVerificationTemplate(new MailAddress("info.soundify@gmail.com"), token);
            new EmailController().SendEmail(mailVerification, email);
        }

        public void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
        }

        public void Resend_Token_Button_Click(object sender, RoutedEventArgs e)
        {
            var mailVerification = new MailVerificationTemplate(new MailAddress("info.soundify@gmail.com"), _previousToken);
            new EmailController().SendEmail(mailVerification, _email);
        }

        public void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var token = this.Token;
            var controller = UserController.Create(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(_email);
            if (token.Text.Equals(_previousToken))
            {
                user.IsActive = true;
                controller.UpdateItem(user);
                MainWindow.InstanceLoginScreen.Show();
                this.Close();
            }
            else
            {
                this.Error.Content = "Tokens do not match";
            }
        }

        public void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceLoginScreen.Show();
            this.Close();
        }
        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
