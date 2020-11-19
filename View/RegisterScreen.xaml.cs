using System;
using System.Collections.Generic;
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
using Model.Data;
using Model.DbModels;
using Model.Enums;
using Soundify;

namespace View
{
    /// <summary>
    /// Interaction logic for RegisterScreen.xaml
    /// </summary>
    public partial class RegisterScreen : Window
    {
        public RegisterScreen()
        {
            InitializeComponent();
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            var username = this.UsernameRegister.Text;
            var email = this.EmailRegister.Text;
            var password = this.PasswordRegister.Password;
            var passwordRepeat = this.PasswordConfirmRegister.Password;

            var result =
                new UserController(new DatabaseContext()).CreateAccount(new User() {Email = email, Username = username},
                    password, passwordRepeat);

            switch(result)
            {
                case RegistrationResults.Succeeded:
                {
                    var loginScreen = new LoginScreen();
                    loginScreen.Show();
                    this.Close();
                    break;
                }
                case RegistrationResults.EmailTaken:
                {
                    this.Error.Content = "Email has already been taken";
                    this.EmailRegister.Text = "";
                    break;
                }
                case RegistrationResults.PasswordNoMatch:
                {
                    this.Error.Content = "Passwords do not match";
                    this.PasswordRegister.Password = "";
                    this.PasswordConfirmRegister.Password = "";
                    break;
                }
                case RegistrationResults.PasswordNotStrongEnough:
                {
                    this.Error.Content = $"Password is {PasswordController.CheckStrength(password).ToString()}";
                    this.PasswordRegister.Password = "";
                    this.PasswordConfirmRegister.Password = "";
                    break;
                }

            }
        }
    }
}
