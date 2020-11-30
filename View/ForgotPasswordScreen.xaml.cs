using System;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace View
{
    /// <summary>
    /// Interaction logic for ForgotPasswordScreen.xaml
    /// </summary>
    public partial class ForgotPasswordScreen : Window
    {
        public ForgotPasswordScreen()
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
            var token = this.Token;
            var newPassword = this.NewPassword.Password;
            var repeatPassword = this.RepeatPassword.Password;

            var controller = new UserController(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(DataContexts.DataContext.Instance.CurrentUser.Email);

            if (token.Text.Equals("something"))
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

        }
    }
}
