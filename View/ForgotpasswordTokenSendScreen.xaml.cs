using System;
using System.Net.Mail;
using System.Windows;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.MailTemplates;
using Soundify;

namespace View
{
    /// <summary>
    /// Interaction logic for ForgotpasswordTokenSendScreen.xaml
    /// </summary>
    public partial class ForgotpasswordTokenSendScreen : Window
    {
        public ForgotpasswordTokenSendScreen()
        {
            InitializeComponent();

            // Makes screen in universal font
            Style = (Style)FindResource(typeof(Window));
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceLoginScreen.Show();
            this.Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var email = Email.Text;
            var token = Guid.NewGuid().ToString();

            var emailController = new EmailController();
            var mail = new ForgotPasswordTemplate(new MailAddress("info.soundify@gmail.com"), token);

            if (email != "" && email.Contains(".") && email.Contains("@"))
                emailController.SendEmail(mail, email);
            else
            {
                this.Error.Content = "You must enter a valid email address to continue";
                return;
            }

            var controller = UserController.Create(DatabaseContext.Instance);
            var user = controller.GetUserFromEmailOrUsername(email);

            if (user == null)
            {
                this.Error.Content = "This email was not recognized, please try again";
                return;
            }

            user.Token = token;
            controller.UpdateItem(user);

            var forgotPasswordScreen = new ForgotPasswordScreen(email);
            this.Close();
            forgotPasswordScreen.Show();
            forgotPasswordScreen.Focus();
        }
    }
}
