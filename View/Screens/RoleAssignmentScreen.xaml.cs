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
                    .GetFilteredList(u => u.Username.ToLower().Contains(textBox.Text.ToLower()) && u.ID != UserController.CurrentUser.ID);
                RoleAssignmentDataContext.Instance.OnPropertyChanged();
            }
        }

        private void UserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = (User)((ContentPresenter)((ComboBox)sender).TemplatedParent).Content;
            int roleId = ((ComboBox)sender).SelectedIndex + 1;
            var role = RoleController.Create(DatabaseContext.Instance).GetItem(roleId);
            var artistController = ArtistController.Create(DatabaseContext.Instance);
            if (user.RoleID != roleId)
            {
                if (user.RoleID == 1) artistController.CreateItem(new Artist {ArtistName = user.Username, UserID = user.ID});
                if (roleId == 1) artistController.RevokeArtist(artistController.GetArtistFromUser(user));

                UserController.Create(DatabaseContext.Instance).UpdateUserRole(user, roleId);

                RoleAssignmentDataContext.Instance.UpdateStatus = $"{user.Username} has been made {role.Designation}";
            }
            RoleAssignmentDataContext.Instance.OnPropertyChanged();
        }
    }
}
