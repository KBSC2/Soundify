using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using Controller;
using Model.Annotations;
using Model.DbModels;

namespace View.DataContexts
{
    public class SongAlterationDataContext : INotifyPropertyChanged
    {
        private static SongAlterationDataContext instance;
        public static SongAlterationDataContext Instance => instance ??= new SongAlterationDataContext();

        public Song Song { get; set; }
        public string ArtistName { get; set; }
        public TimeSpan Duration { get; set; }
        public BitmapImage ImageSource { get; set; }

        public void SetSong(Song song)
        {
            Song = song;

            ArtistName = Song.Artist.ArtistName;
            Duration = TimeSpan.FromSeconds(Song.Duration);

            ImageSource = new BitmapImage(new Uri(FileCache.Instance.GetFile(song.PathToImage)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
