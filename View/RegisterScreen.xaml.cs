using System.Windows;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

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
                    this.Hide();
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
        private void BackToLogin_Button_Click(object sender, RoutedEventArgs e)
        {
            var cancelScreen = new LoginScreen();
            this.Close();
            cancelScreen.Show();
            cancelScreen.Focus();
        }
    }
}
