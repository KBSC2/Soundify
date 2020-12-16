using System.Windows;
using System.Windows.Controls;
using Controller;
using Controller.DbControllers;
using Model.Enums;
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
            DataContexts.DataContext.Instance.Timer.Stop();
            UserController.CurrentUser = null;
            AudioPlayer.Instance.ResetAudioPlayer();

            MainWindow.InstanceMainWindow.Hide();
            MainWindow.InstanceLoginScreen.Show();
            MainWindow.InstanceLoginScreen.RememberData.IsChecked = false;
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.HomeScreen);
        }

        private void ChangePassword_Button_Click(object sender, RoutedEventArgs e)
        {
            var change = new ChangePasswordScreen();
                change.Show();
                change.Focus();
        }

        private void VolumeComboBox_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int selectedItem = comboBox.SelectedIndex;

            AudioPlayer.Instance.ChangeVolume(selectedItem);
        }
        
        private void Request_Button_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestArtist();
            request.Show();
            request.Focus();
        }
    }
}
