using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class PlaylistMenuDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static PlaylistMenuDataContext _instance;
        public static PlaylistMenuDataContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlaylistMenuDataContext();
                return _instance;
            }
        }

        public List<Playlist> PlaylistsSource => PlaylistController.Create(DatabaseContext.Instance).GetActivePlaylists(UserController.CurrentUser.ID);

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}