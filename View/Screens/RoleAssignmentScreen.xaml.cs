using Controller.DbControllers;
using Model.Database.Contexts;
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
                var textBox = (TextBox)sender;
                var text = textBox.Text;
                var Users = UserController.Create(DatabaseContext.Instance).GetList()
                    .Where(u => u.Username.ToLower().Contains(text.ToLower()));
                var userRoles = new List<UserRoles>();
                Users.ToList().ForEach(u => userRoles.Add(new UserRoles(u)));
                RoleAssignmentDataContext.Instance.UserRoles = userRoles;
                RoleAssignmentDataContext.Instance.OnPropertyChanged();
            }
        }

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserRoles userRole = (UserRoles)((ContentPresenter)((ComboBox)sender).TemplatedParent).Content;
            int roleID = ((ComboBox)sender).SelectedIndex +1;
            var role = RoleController.Create(DatabaseContext.Instance).GetItem(roleID);

            if (userRole.UserID != UserController.CurrentUser.ID)
                UserController.Create(DatabaseContext.Instance).UpdateUserRole(userRole.UserID, roleID);

            RoleAssignmentDataContext.Instance.UpdateStatus = $"{userRole.Username} has been made {role.Designation}";
            RoleAssignmentDataContext.Instance.OnPropertyChanged();
        }
    }
}
