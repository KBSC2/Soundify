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
            var password = this.PasswordRegister.Text;
            var passwordRepeat = this.PasswordConfirmRegister.Text;

            var result = new UserController(new DatabaseContext()).CreateAccount(new User() {Email = email, Username = username}, password, passwordRepeat);

            switch(result)
            {
                case RegistrationResults.Succeeded:
                {
                    var loginScreen = new LoginScreen();
                    loginScreen.Show();
                    this.Close();
                    break;
                }
            }
        }
    }
}
