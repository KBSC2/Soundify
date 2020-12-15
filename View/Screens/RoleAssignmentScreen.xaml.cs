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
using View.ListItems;

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
        }

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int userID = ((UserRoles)((ContentPresenter)((ComboBox)sender).TemplatedParent).Content).UserID;
            int userrole = ((ComboBox)sender).SelectedIndex;

            userrole += 1;
            
            if(userID != UserController.CurrentUser.ID)
                UserController.Create(new DatabaseContext()).UpdateUserRole(userID, userrole);
        }
    }
}
