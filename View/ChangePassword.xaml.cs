using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Controller;
using Controller.DbControllers;
using Model;
using Model.Data;
using Model.Enums;
using Soundify;

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
            var currentPassword = this.CurrentPassword.Text;
            var newPassword = this.NewPassword.Text;
            var repeatPassword = this.RepeatPassword.Text;

            var controller = new UserController(new DatabaseContext());
            var user = controller.GetUserFromEmailOrUsername(View.DataContext.Instance.CurrentUser.Email);
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
                    break;
                }

                case LoginResults.PasswordIncorrect:
                {
                    this.Error.Content = "Password incorrect";
                    break;
                }
            }
            this.Close();
        }
    }
}
