using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.Enums;
using Model.MailTemplates;
using Soundify;

namespace View
{
    /// <summary>
    /// Interaction logic for ForgotPasswordScreen.xaml
    /// </summary>
    public partial class ForgotPasswordScreen : Window
    {
        private string email;
        private string token;

        public ForgotPasswordScreen(string text)
        {
            InitializeComponent();
            email = text;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceLoginScreen.Show();
            this.Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        { 
            token = this.Token.Text;
            var newPassword = this.NewPassword.Password;
            var repeatPassword = this.RepeatPassword.Password;

            var controller = UserController.Create(DatabaseContext.Instance);
            var user = controller.GetUserFromEmailOrUsername(email);

            if (token.Equals(user.Token))
            {
                if (newPassword == "" || repeatPassword == "")
                {
                    this.Error.Content = "Password fields cannot be blank";
                    return;
                }
                if (!newPassword.Equals(repeatPassword))
                {
                    this.Error.Content = "Passwords don't match";
                    return;
                }

                if (PasswordController.CheckStrength(newPassword) < PasswordScore.Strong)
                {
                    this.Error.Content = "Password is too weak";
                    return;
                }

                user.Password = PasswordController.EncryptPassword(newPassword);
                controller.UpdateItem(user);
                this.Close();
                MainWindow.InstanceLoginScreen.Show();
            }
            else
            {
                this.Error.Content = "Token does not match";
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
            this.Error.Content = "New token has been sent!";
            var emailController = new EmailController();
            var mail = new ForgotPasswordTemplate(new MailAddress("info.soundify@gmail.com"), token);

            if (email.Contains(".") && email.Contains("@"))
                emailController.SendEmail(mail, email);
        }
    }
}
