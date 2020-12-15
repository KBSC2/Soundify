using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.Enums;

namespace View
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePasswordScreen : Window
    {
        public ChangePasswordScreen()
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

            var controller = UserController.Create(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(UserController.CurrentUser.Email);
            var result = controller.UserLogin(user.Email, currentPassword);
            switch (result)
            {
                case LoginResults.Success:
                {
                    if (newPassword == "" || repeatPassword == "")
                    {
                        //Checks whether one or two fields are blank, depending on that the error content becomes singular or plural
                        var s = (newPassword == "" && repeatPassword == "") ? "s are" : " is";
                        this.Error.Content = $"Password field{s} blank";
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
                    break;
                }

                case LoginResults.PasswordIncorrect:
                {
                    this.Error.Content = "Password incorrect";
                    break;
                }
            }
        }

        private void Confirm_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Confirm_Button_Click(sender, new RoutedEventArgs());
            }
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
