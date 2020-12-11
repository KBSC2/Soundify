﻿using System.Windows;
using Controller;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using View.DataContexts;
using System.Windows.Input;
using Model;
using Model.EventArgs;
using Soundify;
using System;

namespace View.Screens
{
    partial class PlaylistScreen : ResourceDictionary
    {
        public static PlaylistScreen Instance { get; set; }

        public PlaylistScreen()
        {
            this.InitializeComponent();
            Instance = this;
        }

        private void ListViewItem_RightClickSongInfo(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;

            SongInfoScreen songInfoScreen = new SongInfoScreen(song);
            songInfoScreen.Show();
        }

        public void ListViewItem_RightClick_DeleteSong(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;
            var playlist = MainWindow.CurrentPlayList;
            PlaylistSongController.Create(new DatabaseContext()).RemoveFromPlaylist(song.ID, playlist.ID);

            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

        public void ListViewItem_RightClickAddQueue(object sender, RoutedEventArgs e)
        {
            var song = ((SongInfo)((MenuItem)sender).DataContext).Song;

            AudioPlayer.Instance.AddSongToSongQueue(song);
        }

        private void Play_Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList);
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {

            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList, songInfo.Index-1);
        }

        private void RemovePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlistID = (int)((Button)sender).Tag;

            var playlistController = PlaylistController.Create(new DatabaseContext());
            var playlistName = playlistController.GetItem(playlistID).Name;

            var removeConfirm = MessageBox.Show($"Are you sure you want to delete {playlistName}?", $"Remove {playlistName.ToString()}", MessageBoxButton.YesNoCancel);
            switch (removeConfirm)
            {
                case MessageBoxResult.Yes:
                    playlistController.DeactivatePlaylist(playlistID);
                    MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs() { ScreenName = ScreenNames.PlaylistMenuScreen } );
                    break;
                case MessageBoxResult.No:
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var selectedSongInfo = (SongInfo)((ListView)mainGrid.FindName("SongList"))?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index - 1 < 0) return;


            SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index - 1);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index + 1 >= listView.Items.Count) return;

            SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index + 1);
        }

        public void SwapSongs(int indexOne, int indexTwo)
        {
            var playlistSongController = PlaylistSongController.Create(new DatabaseContext());
            int playlistID = MainWindow.CurrentPlayList.ID;
            var songOne = playlistSongController.GetPlaylistSongFromIndex(playlistID, indexOne);
            var songTwo = playlistSongController.GetPlaylistSongFromIndex(playlistID, indexTwo);

            songOne.Index = indexTwo;
            songTwo.Index = indexOne;

            playlistSongController.UpdatePlaylistSong(songOne);
            playlistSongController.UpdatePlaylistSong(songTwo);

            PlaylistDataContext.Instance.OnPropertyChanged("");
        }
    }
}
