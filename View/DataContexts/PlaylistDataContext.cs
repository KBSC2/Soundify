using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model.Annotations;

namespace View.DataContexts
{
    public class PlaylistDataContext : INotifyPropertyChanged
    {
        private static PlaylistDataContext _instance;
        public static PlaylistDataContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlaylistDataContext();
                return _instance;
            }
        }

        public Playlist Playlist => Soundify.MainWindow.CurrentPlayList;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}