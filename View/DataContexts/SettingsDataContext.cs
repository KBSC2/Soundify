using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class SettingsDataContext : INotifyPropertyChanged
    {
        private static SettingsDataContext instance;
        public static SettingsDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new SettingsDataContext();
                return instance;
            }
        }
        public Role Role => RoleController.Create(new DatabaseContext()).GetItem(UserController.CurrentUser.RoleID);

        public string CurrentUserName => UserController.CurrentUser.Username;

        public string HasArtistRequested => UserController.Create(new DatabaseContext())
            .GetItem(UserController.CurrentUser.ID).RequestedArtist == true
            ? "Hidden"
            : "Visible";

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
