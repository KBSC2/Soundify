using System.Windows;
using Controller;
using View.DataContexts;
using System.Linq;

namespace View.Screens
{
    partial class AlbumSongListScreen : ResourceDictionary
    {
        
        public AlbumSongListScreen()
        {
            this.InitializeComponent();
        }

        private void Play_Album_Button_Click(object sender, RoutedEventArgs e)
        {
            var songList = SongListDataContext.Instance.SongInfoList.Select(x => x.Song).ToList();
            AudioPlayer.Instance.PlayAlbum(songList);
        }

        private void AddToQueue_Album_Button_Click(object sender, RoutedEventArgs e)
        {
            var songList = SongListDataContext.Instance.SongInfoList.Select(x => x.Song).ToList();
            songList.ForEach(s => AudioPlayer.Instance.AddSongToQueue(s));
        }

    }
}
