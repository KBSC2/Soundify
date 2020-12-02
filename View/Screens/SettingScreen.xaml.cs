using System.Windows;
using System.Windows.Controls;
using Controller;
using Controller.DbControllers;
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
            UserController.CurrentUser = null;
            MainWindow.InstanceMainWindow.Hide();
            MainWindow.InstanceLoginScreen.Show();
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

            switch (selectedItem) 
            {
                case 0:
                    AudioPlayer.Instance.MaxVolume = 0.1;
                    break;
                case 2:
                    AudioPlayer.Instance.MaxVolume = 0.4;
                    break;
                default:
                    AudioPlayer.Instance.MaxVolume = 0.2;
                    break;
            }
        }
        
        private void Request_Button_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestArtist();
                request.Show();
                request.Focus();
        }
    }
}
