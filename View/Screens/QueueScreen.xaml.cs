using Controller;
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

        private void Queue_SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            var index = AudioPlayer.Instance.Queue.IndexOf(songInfo.Song);

            AudioPlayer.Instance.PlayQueue(index-1);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Queue_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("Queue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex - 1 < 0) return;

            QueueController.Instance.SwapSongs(listView.SelectedIndex + 1, listView.SelectedIndex, AudioPlayer.Instance.Queue);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Queue_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("Queue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex + 1 >= listView.Items.Count) return;

            QueueController.Instance.SwapSongs(listView.SelectedIndex + 1, listView.SelectedIndex + 2, AudioPlayer.Instance.Queue);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        

        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            QueueController.Instance.DeleteSongFromQueue(((SongInfo)((MenuItem)sender).DataContext).Song);
            QueueDataContext.Instance.OnPropertyChanged();
        }
    }
}
