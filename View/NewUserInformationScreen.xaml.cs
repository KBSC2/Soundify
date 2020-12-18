using System;
using System.Windows;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.Enums;
using View.DataContexts;

namespace View
{
    /// <summary>
    /// Interaction logic for NewUserInformationScreen.xaml
    /// </summary>

    public partial class NewUserInformationScreen : Window
    {
        public NewUserInformationScreen()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Minimize_Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            var currentUserEmail = UserController.CurrentUser.Email;
            var currentPasswordBox = this.CurrentPassword.Password;
            var passwordResult = UserController.Create(DatabaseContext.Instance).UserLogin(currentUserEmail, currentPasswordBox);

            switch (passwordResult)
            {
                case LoginResults.Success:
                    var currentUser = UserController.CurrentUser;
                    var detailsResult = UserController.Create(DatabaseContext.Instance)
                        .ChangeDetails(currentUser, NewEmail.Text, NewUsername.Text);
                    switch (detailsResult)
                    {
                        case NewUserInfo.Valid:
                            SettingsDataContext.Instance.OnPropertyChanged("CurrentUserName");
                            Close();
                            break;
                        case NewUserInfo.UsernameTakenEmailUpdated:
                            NewEmail.IsEnabled = false;
                            NewEmail.Text = "";
                            NewEmail.Tag = "  Email changed";
                            NewEmail.TextAlignment = TextAlignment.Center;
                            Error.Content = "Your username has been taken, email has been updated.";
                            break;
                        case NewUserInfo.EmailTaken:
                            NewEmail.Text = "";
                            Error.Content = "Your email has been taken";
                            break;
                        case NewUserInfo.InvalidEmail:
                            NewEmail.Text = "";
                            Error.Content = "Please put in a valid email";
                            break;
                        case NewUserInfo.UsernameTaken:
                            NewUsername.Text = "";
                            Error.Content = "Your username has been taken";
                            break;
                        case NewUserInfo.Empty:
                            Error.Content = "Please fill in the form";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case LoginResults.PasswordIncorrect:
                    CurrentPassword.Password = "";
                    this.Error.Content = "Password is incorrect!";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
