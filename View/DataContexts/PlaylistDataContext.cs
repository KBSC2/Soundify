using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.DbModels;
using Model.Annotations;
using Model.Data;

namespace View.DataContexts
{
    public class PlaylistDataContext : INotifyPropertyChanged
    {
        public List<SongInfo> PlaylistItems => SongInfo.ConvertSongListToSongInfo(Soundify.MainWindow.CurrentPlayList,
            new PlaylistSongController(new DatabaseContext()).GetSongsFromPlaylist(Soundify.MainWindow.CurrentPlayList.ID));
        public Playlist Playlist => Soundify.MainWindow.CurrentPlayList;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}