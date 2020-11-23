using System.Windows;
using Controller.DbControllers;
using Model.Data;
using Model.Enums;
using Soundify;

namespace View
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            var emailOrUsername = this.UsernameLogin.Text;
            var password = this.PasswordLogin.Password;

            var controller = new UserController(new DatabaseContext());
            var result = controller.UserLogin(emailOrUsername, password);
            switch (result)
            {
                case LoginResults.Success:
                {
                    DataContexts.DataContext.Instance.CurrentUser = controller.GetUserFromEmailOrUsername(emailOrUsername);
                    var main = new MainWindow();
                    main.Show();
                    main.Focus();
                    this.Hide();
                    this.UsernameLogin.Text = "Email";
                    this.PasswordLogin.Password = "Password";
                    break;
                }
                case LoginResults.EmailNotFound:
                {
                    this.Error.Content = "Email not found";
                    break;
                }
                case LoginResults.PasswordIncorrect:
                {
                    this.Error.Content = "Password incorrect";
                    break;
                }
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var signupScreen = new RegisterScreen();
            this.Close();
            signupScreen.Show();
            signupScreen.Focus();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
