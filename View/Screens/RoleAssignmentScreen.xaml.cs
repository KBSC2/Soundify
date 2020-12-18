using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public void GetUsers(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                RoleAssignmentDataContext.Instance.Users = UserController.Create(DatabaseContext.Instance)
                    .GetFilteredList(u => u.Username.ToLower().Contains(((TextBox)sender).Text.ToLower()) && u.ID != UserController.CurrentUser.ID);
                RoleAssignmentDataContext.Instance.OnPropertyChanged();
            }
        }

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (User)((ContentPresenter)((ComboBox)sender).TemplatedParent).Content;
            int roleID = ((ComboBox)sender).SelectedIndex + 1;
            if(user.RoleID != roleID)
            {
                if (user.RoleID == 1) artistController.CreateItem(new Artist {ArtistName = user.Username, UserID = user.ID});
                if (roleId == 1) artistController.RevokeArtist(artistController.GetArtistFromUser(user));
                
                UserController.Create(DatabaseContext.Instance).UpdateUserRole(user, roleID);

                RoleAssignmentDataContext.Instance.UpdateStatus = $"{user.Username} has been made {RoleController.Create(DatabaseContext.Instance).GetItem(roleID).Designation}";
            }
            RoleAssignmentDataContext.Instance.OnPropertyChanged();
        }
    }
}
