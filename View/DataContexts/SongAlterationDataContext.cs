using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Controller;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class SongAlterationDataContext : INotifyPropertyChanged
    {
        private static SongAlterationDataContext instance;
        public static SongAlterationDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new SongAlterationDataContext();
                return instance;
            }
        }

        public Song Song { get; set; }
        public string ArtistName { get; set; }
        public TimeSpan Duration { get; set; }
        public BitmapImage ImageSource { get; set; }

        public void SetSong(Song song)
        {
            Song = song;

            var artistConroller = ArtistController.Create(new DatabaseContext());
            var artist = artistConroller.GetItem(Song.Artist);
            ArtistName = artist.ArtistName;
            Duration = TimeSpan.FromSeconds(Song.Duration);
            ImageSource = new BitmapImage(new Uri(FileCache.Instance.GetFile(song.PathToImage)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
