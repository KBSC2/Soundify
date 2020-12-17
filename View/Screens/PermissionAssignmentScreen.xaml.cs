using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for PermissionAssignmentScreen.xaml
    /// </summary>
    public partial class PermissionAssignmentScreen : ResourceDictionary
    {
        private static PermissionAssignmentScreen instance;
        public static PermissionAssignmentScreen Instance
        {
            get
            {
                if (instance == null)
                    instance = new PermissionAssignmentScreen();
                return instance;
            }
        }
        public PermissionAssignmentScreen()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var roleID = int.Parse((string)((CheckBox)sender).Tag);
            var permission = ((KeyValuePair<Permission, bool[]>)((CheckBox)sender).DataContext).Key;
            RolePermissionsController.Create(DatabaseContext.Instance).AddPermissionToRole(permission.ID, roleID);
            MessageBox.Show("The permission was added successfully to the selected role");
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            var roleID = int.Parse((string)((CheckBox)sender).Tag);
            var permission = ((KeyValuePair<Permission, bool[]>)((CheckBox)sender).DataContext).Key;
            RolePermissionsController.Create(DatabaseContext.Instance).RemovePermissionFromRole(permission.ID, roleID);
            MessageBox.Show("The permission was removed successfully from the selected role");
        }
    }
}