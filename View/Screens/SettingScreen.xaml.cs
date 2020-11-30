using System.Windows;
using Soundify;
using View.DataContexts;

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
            DataContext.Instance.CurrentUser = null;
            MainWindow.InstanceMainWindow.Hide();
            MainWindow.InstanceLoginScreen.Show();
        }

        private void ChangePassword_Button_Click(object sender, RoutedEventArgs e)
        {
            var change = new ChangePasswordScreen();
                change.Show();
                change.Focus();
        }
    }
}
