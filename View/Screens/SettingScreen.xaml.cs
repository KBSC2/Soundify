using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Soundify;

namespace View.Screens
{
    partial class SettingScreen : ResourceDictionary
    {
        public SettingScreen()
        {
            this.InitializeComponent();
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Hide();
            DataContext.Instance.CurrentUser = null;
            var login = new LoginScreen();
            login.Show();
            login.Focus();
        }

        private void ChangePassword_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
