﻿using Controller;
using Model.DbModels;
using Soundify;
using System.Collections.Generic;
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

            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, index-1);
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Queue_MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("Queue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex - 1 < 0) return;

            SwapSongs(listView.SelectedIndex, listView.SelectedIndex - 1, AudioPlayer.Instance.Queue);
        }

        private void Queue_MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView)mainGrid.FindName("Queue");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || listView.SelectedIndex + 1 >= listView.Items.Count) return;

            SwapSongs(listView.SelectedIndex, listView.SelectedIndex + 1, AudioPlayer.Instance.Queue);
        }

        public void SwapSongs(int indexOne, int indexTwo, List<Song> list)
        {
            var listitem1 = list[indexOne+1];
            var listitem2 = list[indexTwo+1];

            list[indexOne+1] = listitem2;
            list[indexTwo+1] = listitem1;

            QueueDataContext.Instance.OnPropertyChanged();
        }

        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;

            AudioPlayer.Instance.Queue.Remove(song);
            AudioPlayer.Instance.NextInQueue.Remove(song);

            QueueDataContext.Instance.OnPropertyChanged();
        }
    }
}
