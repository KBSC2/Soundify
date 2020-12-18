using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model.Annotations;
using Controller;
using System;

namespace View.DataContexts
{
    public class PlaylistDataContext : INotifyPropertyChanged
    {
        private static PlaylistDataContext instance;
        public static PlaylistDataContext Instance => instance ??= new PlaylistDataContext();

        public Playlist Playlist => Soundify.MainWindow.CurrentPlayList;

        public event PropertyChangedEventHandler PropertyChanged;

        private PlaylistDataContext()
        {
            AudioPlayer.Instance.NextSong += OnNextSong;
        }

        public void OnNextSong(object sender, EventArgs e)
        {
            SongListDataContext.Instance.OnPropertyChanged();
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}