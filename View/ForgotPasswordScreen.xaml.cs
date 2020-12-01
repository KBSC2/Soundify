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
    /// Interaction logic for ForgotPasswordScreen.xaml
    /// </summary>
    public partial class ForgotPasswordScreen : Window
    {
        private string _email;
        private string _token;
        public ForgotPasswordScreen(string text)
        {
            InitializeComponent();
            _email = text;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            var cancelScreen = new LoginScreen();
            this.Close();
            cancelScreen.Show();
            cancelScreen.Focus();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var token = this.Token.Text;
            var newPassword = this.NewPassword.Password;
            var repeatPassword = this.RepeatPassword.Password;

            var controller = UserController.Create(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(_email);

            if (token.Equals(user.Token))
            {
                if (!newPassword.Equals(repeatPassword))
                {
                    this.Error.Content = "Passwords don't match";
                    return;
                }

                if (PasswordController.CheckStrength(newPassword) < PasswordController.PasswordScore.Strong)
                {
                    this.Error.Content = "Password is too weak";
                    return;
                }

                user.Password = PasswordController.EncryptPassword(newPassword);
                controller.UpdateItem(user);
                this.Close();
                MainWindow.InstanceLoginScreen.Show();
            }
        }

        private void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void Resend_Token_Button_Click(object sender, RoutedEventArgs e)
        {
            var emailController = new EmailController<ForgotPasswordTemplate>();
            var mail = new ForgotPasswordTemplate(new MailAddress("info.soundify@gmail.com"), _token);

            if (_email.Contains(".") && _email.Contains("@"))
                emailController.SendEmail(mail, _email);
        }
    }
}
