using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Model;
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

        private void OpenRoleAssignmentScreen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.InstanceMainWindow.SetScreen(ScreenNames.RoleAssignmentScreen);
        }
    }
}