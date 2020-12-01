﻿using System;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.MailTemplates;
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
            var token = Guid.NewGuid().ToString();

            var result =
                new UserController(new DatabaseContext()).CreateAccount(new User() {Email = email, Username = username, Token = token},
                    password, passwordRepeat);

            switch(result)
            {
                case RegistrationResults.Succeeded:
                {
                    var emailVerificationScreen = new EmailVerificationScreen(token, email);
                    emailVerificationScreen.Show();
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

            var mailVerification = new MailVerificationTemplate(new MailAddress("info.soundify@gmail.com"), token);
            new EmailController<MailVerificationTemplate>().SendEmail(mailVerification, email);
        }
        private void Register_On_Enter_Key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Register_Button_Click(sender, new RoutedEventArgs());
            }
        }
        private void BackToLogin_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceLoginScreen.Show();
            this.Close();
        }
    }
}
