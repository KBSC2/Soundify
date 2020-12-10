using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class PlaylistMenuDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static PlaylistMenuDataContext instance;
        public static PlaylistMenuDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlaylistMenuDataContext();
                return instance;
            }
        }

        public List<Playlist> PlaylistsSource {get; set; } = new List<Playlist>();

        public PlaylistMenuDataContext()
        {
            UpdatePlaylists();
        }

        public void UpdatePlaylists()
        {
            PlaylistController.Create(new DatabaseContext()).GetActivePlaylists(UserController.CurrentUser.ID).ContinueWith(res =>
            {
                PlaylistsSource = res.Result;
                OnPropertyChanged("");
            });
        }


        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}