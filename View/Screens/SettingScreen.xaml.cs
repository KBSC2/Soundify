using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Text;
using System.Windows;
using Soundify;

namespace View.Screens
{
    partial class SettingScreen : ResourceDictionary
    {
        public bool IsOpen = false;

        public SettingScreen()
        {
            this.InitializeComponent();
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            DataContext.Instance.CurrentUser = null;
            MainWindow.InstanceMainWindow.Hide();
            MainWindow.InstanceLoginScreen.Show();
        }

        private void ChangePassword_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOpen)
            {
                var change = new ChangePassword();
                change.Show();
                change.Focus();
            }
        }
    }
}
