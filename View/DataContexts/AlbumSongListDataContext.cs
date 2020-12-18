using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model.Annotations;
using Controller;

namespace View.DataContexts
{
    public class AlbumSongListDataContext : INotifyPropertyChanged
    {
        private static AlbumSongListDataContext instance;
        public static AlbumSongListDataContext Instance => instance ??= new AlbumSongListDataContext();

        public Album Album { get; set; }
        
        public string PathToImage => Album == null ? "../Assets/null.png" : Album.PathToImage == null ? "../Assets/null.png" : FileCache.Instance.GetFile(Album.PathToImage);

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}