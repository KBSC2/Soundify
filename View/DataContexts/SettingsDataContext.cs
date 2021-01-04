using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller;
using Controller.DbControllers;
using Model.Annotations;
using Model.DbModels;

namespace View.DataContexts
{
    public class SettingsDataContext : INotifyPropertyChanged
    {
        private static SettingsDataContext instance;
        public static SettingsDataContext Instance => instance ??= new SettingsDataContext();
        public Role Role => UserController.CurrentUser.Role;

        public int VolumeComboBoxSelectedIndex => AudioPlayer.Instance.MaxVolume == 0.1 ? 0 : AudioPlayer.Instance.MaxVolume == 0.4 ? 2 : 1;

        public string CurrentUserName => UserController.CurrentUser.Username;

        public string HasArtistRequested => UserController.CurrentUser.RequestedArtist
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
