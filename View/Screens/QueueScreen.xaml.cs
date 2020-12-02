using Controller;
using Soundify;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using View.DataContexts;

namespace View.Screens
{
    partial class QueueScreen : ResourceDictionary
    {
        public QueueScreen()
        {
            this.InitializeComponent();
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            var index = AudioPlayer.NextInPlaylist.IndexOf(songInfo.Song);

            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, index-1);
            QueueDataContext.Instance.OnPropertyChanged();
        }
    }
}
