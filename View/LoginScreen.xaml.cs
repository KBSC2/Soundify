using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
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
            if(File.Exists(Path.GetTempPath() + "Soundify/settings/loginInfo"))
            {
                string text = File.ReadAllText(Path.GetTempPath() + "Soundify/settings/loginInfo");
                if (text != "")
                {
                    string[] split = text.Split(",");

                    UsernameLogin.Text = split[1];
                    PasswordLogin.Password = split[0];
                    RememberData.IsChecked = true;
                }
            }
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            var emailOrUsername = this.UsernameLogin.Text;
            var password = this.PasswordLogin.Password;

            var controller = UserController.Create(new DatabaseContext());
            var result = controller.UserLogin(emailOrUsername, password);
            switch (result)
            {
                case LoginResults.Success:
                {
                    UserController.CurrentUser = controller.GetUserFromEmailOrUsername(emailOrUsername);
                    var main = MainWindow.InstanceMainWindow;
                    main.Show();
                    main.Focus();
                    this.Hide();
                    File.Create(Path.GetTempPath() + "Soundify/settings/loginInfo").Close();
                    MainWindow.InstanceMainWindow.UpdateButtons();

                    if (RememberData.IsChecked ?? false)
                    {
                        string path = Path.GetTempPath() + "Soundify/settings/loginInfo";
                        string text = password + "," + emailOrUsername;
                        File.WriteAllText(path, text);
                    }

                    this.UsernameLogin.Text = "";
                    this.PasswordLogin.Password = "";
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
                case LoginResults.UserNotActive:
                {
                    var token = Guid.NewGuid().ToString();
                    var user = controller.GetUserFromEmailOrUsername(emailOrUsername);
                    if (user != null)
                    {
                        var userEmail = user.Email;
                        var emailVerificationScreen = new EmailVerificationScreen(token, userEmail);
                        emailVerificationScreen.Error.Content = "User not active";
                        emailVerificationScreen.Show();
                        this.Hide();
                    } 
                    break;
                }
            }
        }

        private void Login_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var signupScreen = new RegisterScreen();
            this.Hide();
            signupScreen.Show();
            signupScreen.Focus();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            var forgotPasswordTokenSendScreen = new ForgotpasswordTokenSendScreen();
            this.Hide();
            forgotPasswordTokenSendScreen.Show();
            forgotPasswordTokenSendScreen.Focus();
        }

        private void WindowClosing(object sender, EventArgs e)
        {
            SSHController.Instance.CloseSSHTunnel();
            Application.Current.Shutdown();
            MainWindow.InstanceMainWindow?.Close();
        }
    }
}
