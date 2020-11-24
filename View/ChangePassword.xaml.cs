using System.Windows;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.Enums;

namespace View
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var currentPassword = this.CurrentPassword.Password;
            var newPassword = this.NewPassword.Password;
            var repeatPassword = this.RepeatPassword.Password;

            var controller = new UserController(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(DataContexts.DataContext.Instance.CurrentUser.Email);
            var result = controller.UserLogin(user.Email, currentPassword);
            switch (result)
            {
                case LoginResults.Success:
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
                    break;
                }

                case LoginResults.PasswordIncorrect:
                {
                    this.Error.Content = "Password incorrect";
                    break;
                }
            }
        }
    }
}
