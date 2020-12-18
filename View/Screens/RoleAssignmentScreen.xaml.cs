using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        }

        public void GetUsers(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textBox = (TextBox)sender;

                RoleAssignmentDataContext.Instance.Users = UserController.Create(DatabaseContext.Instance)
                    .GetFilteredList(u => u.Username.ToLower().Contains(textBox.Text.ToLower()));
                RoleAssignmentDataContext.Instance.OnPropertyChanged();
            }
        }

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (User)((ContentPresenter)((ComboBox)sender).TemplatedParent).Content;
            int roleId = ((ComboBox)sender).SelectedIndex + 1;
            var role = RoleController.Create(DatabaseContext.Instance).GetItem(roleId);
            if(user.RoleID != roleId)
            {
                UserController.Create(DatabaseContext.Instance).UpdateUserRole(user, roleId);

                RoleAssignmentDataContext.Instance.UpdateStatus = $"{user.Username} has been made {role.Designation}";
            }
            RoleAssignmentDataContext.Instance.OnPropertyChanged();
        }
    }
}
