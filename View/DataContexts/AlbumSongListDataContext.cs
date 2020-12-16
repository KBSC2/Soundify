using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model.Annotations;
using Model.Database.Contexts;
using Controller;
using System;
using System.Linq;
using Controller.DbControllers;

namespace View.DataContexts
{
    public class AlbumSongListDataContext : INotifyPropertyChanged
    {
        private static AlbumSongListDataContext instance;
        public static AlbumSongListDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new AlbumSongListDataContext();
                return instance;
            }
        }

        public Album Album { get; set; }

        public Artist Artist => ArtistController.Create(DatabaseContext.Instance).GetItem(Album.AlbumArtistSongs
            .Where(aas => aas.Album.ID.Equals(Album.ID)).ToList()
            .GroupBy(aas => aas.ArtistId)
            .OrderBy(aas => aas.Count())
            .Select(aas => aas.Key).First());

        public string PathToImage => Album == null ? "../Assets/null.png" : Album.PathToImage == null ? "../Assets/null.png" : FileCache.Instance.GetFile(Album.PathToImage);

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}