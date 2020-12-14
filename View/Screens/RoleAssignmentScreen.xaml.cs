using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for RoleAssignmentScreen.xaml
    /// </summary>
    public partial class RoleAssignmentScreen : ResourceDictionary
    {
        private static RoleAssignmentDataContext instance;
        public static RoleAssignmentDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoleAssignmentDataContext();
                return instance;
            }
        }
        public RoleAssignmentScreen()
        {
            InitializeComponent();
        }/*
        private int SetRole(User user)
        {

        }*/
        private void SwapRole_Click(object sender, RoutedEventArgs e)
        {
            var User = (ListView)this.FindName("Users");
            var dataGridRowUser = (DataGridRow)User;
            var selectedUser = (User)dataGridRow.Item;

            var Role = (ListView)this.FindName("Roles");
            var dataGridRowRole = (DataGridRow)Role;
            var selectedRole = (string)dataGridRow.Item;
            selectedUser.RoleID = selectedRole == "User" ? 1 : selectedRole == "Artist" ? 2 : 3 ;
        }
    }
}
