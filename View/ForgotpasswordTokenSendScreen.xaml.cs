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
            var cancelScreen = new LoginScreen();
            this.Close();
            cancelScreen.Show();
            cancelScreen.Focus();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var email = Email.Text;
            var token = Guid.NewGuid().ToString();

            var emailController = new EmailController<ForgotPasswordTemplate>();
            var mail = new ForgotPasswordTemplate(new MailAddress("info.soundify@gmail.com"), token);
            
            if (email.Contains(".") && email.Contains("@"))
                emailController.SendEmail(mail, email);

            var forgotPasswordScreen = new ForgotPasswordScreen();
            this.Hide();
            forgotPasswordScreen.Show();
            forgotPasswordScreen.Focus();
        }

    }
}
