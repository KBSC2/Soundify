using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
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
                if (text != String.Empty)
                {
                    string[] split = text.Split(",");

                    UsernameLogin.Text = split[1];
                    PasswordLogin.Password = split[0];
                }
            }
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
                    File.Create(Path.GetTempPath() + "Soundify/settings/loginInfo").Close();

                    if (RememberData.IsChecked ?? false)
                    {
                        string path = Path.GetTempPath() + "Soundify/settings/loginInfo";
                        string Text = password + "," + emailOrUsername;
                        File.WriteAllText(path, Text);
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

        }
    }
}
