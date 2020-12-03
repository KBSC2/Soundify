using Controller;
using Model.DbModels;
using Soundify;
using System.Collections.Generic;
using System.Diagnostics;
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

        private void NextInPlaylist_SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            var index = AudioPlayer.Instance.NextInPlaylist.IndexOf(songInfo.Song);

            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, index-1, false);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void NextInQueue_SongRow_Click(object sender, MouseButtonEventArgs e)
        {
            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            var index = AudioPlayer.Instance.NextInQueue.IndexOf(songInfo.Song);

            AudioPlayer.Instance.PlayQueue(index);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void NextInPlaylist_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("NextInPlaylist");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;
            var index = listView.SelectedIndex;

            if (listView.Items.Count > AudioPlayer.Instance.NextInPlaylist.Count)
                if (index+1 >= AudioPlayer.Instance.NextInPlaylist.Count)
                    index -= AudioPlayer.Instance.NextInPlaylist.Count;

            if (selectedSongInfo == null || index - 1 < 0) return;

            SwapSongs(index, index - 1, AudioPlayer.Instance.NextInPlaylist);
        }

        private void NextInPlaylist_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("NextInPlaylist");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;
            var index = listView.SelectedIndex;

            if (listView.Items.Count > AudioPlayer.Instance.NextInPlaylist.Count)
            {
                if (index + 1 > listView.Items.Count - AudioPlayer.Instance.NextInPlaylist.Count)
                    index -= AudioPlayer.Instance.NextInPlaylist.Count;
                if (index + 1 >= listView.Items.Count - AudioPlayer.Instance.NextInPlaylist.Count) return;
                if (index < 0) return;
            }
            else 
                if (index + 1 >= listView.Items.Count) return;

            SwapSongs(index, index + 1, AudioPlayer.Instance.NextInPlaylist);
        }

        private void NextInQueue_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("NextInQueue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex - 1 < 0) return;

            SwapSongs(listView.SelectedIndex, listView.SelectedIndex - 1, AudioPlayer.Instance.NextInQueue);
        }

        private void NextInQueue_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("NextInQueue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex + 1 >= listView.Items.Count) return;

            SwapSongs(listView.SelectedIndex, listView.SelectedIndex + 1, AudioPlayer.Instance.NextInQueue);
        }

        public void SwapSongs(int indexOne, int indexTwo, List<Song> list)
        {
            var listitem1 = list[indexOne+1];
            var listitem2 = list[indexTwo+1];

            list[indexOne+1] = listitem2;
            list[indexTwo+1] = listitem1;

            QueueDataContext.Instance.OnPropertyChanged();
        }
    }
}
