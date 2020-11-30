using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow._instanceLoginScreen.Show();
            this.Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var email = Email.Text;
            var token = Guid.NewGuid().ToString();

            var emailController = new EmailController<ForgotPasswordTemplate>();
            var mail = new ForgotPasswordTemplate(new MailAddress("info.soundify@gmail.com"), token);
            
            if (email.Contains(".") && email.Contains("@"))
                emailController.SendEmail(mail, email);

            var controller = new UserController(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(email);
            user.Token = token;
            controller.UpdateItem(user);

            var forgotPasswordScreen = new ForgotPasswordScreen(email);
            this.Close();
            forgotPasswordScreen.Show();
            forgotPasswordScreen.Focus();
        }
    }
}
