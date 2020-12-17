using System.Windows;
using Model.Enums;
using Soundify;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for TempAdminScreen.xaml
    /// </summary>
    public partial class AdminScreen : ResourceDictionary
    {
        public AdminScreen()
        {
            InitializeComponent();
        }

        private void Review_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.RequestScreen);
        }

        private void Permission_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.PermissionAssignmentScreen);
        }

        private void OpenRoleAssignmentScreen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.RoleAssignmentScreen);

        }
    }
}