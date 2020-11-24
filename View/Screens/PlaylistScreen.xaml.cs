﻿using System.Windows;
using Controller;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using View.DataContexts;
using System.Windows.Input;
using Model;
using Model.EventArgs;
using Soundify;

namespace View.Screens
{
    partial class PlaylistScreen : ResourceDictionary
    {
        public PlaylistScreen()
        {
            this.InitializeComponent();
        }

        private void Play_Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.PlayPlaylist(Soundify.MainWindow.CurrentPlayList);
        }

        private void SongRow_Click(object sender, MouseButtonEventArgs e)
        {

            var listViewItem = (ListViewItem)sender;
            var songInfo = (SongInfo)listViewItem.Content;

            AudioPlayer.PlayPlaylist(Soundify.MainWindow.CurrentPlayList, songInfo.Index);
        }

        private void RemovePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlistID = (int)((Button)sender).Tag;

            var playlistController = new PlaylistController(new DatabaseContext());
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


            SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index - 1, mainGrid);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;

            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index + 1 >= listView.Items.Count) return;

            SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index + 1, mainGrid);
        }

        public void SwapSongs(int indexOne, int indexTwo, Grid mainGrid)
        {
            var playlistSongController = new PlaylistSongController(new DatabaseContext());
            int playlistID = Soundify.MainWindow.CurrentPlayList.ID;
            var songOne = playlistSongController.GetPlaylistSongFromIndex(playlistID, indexOne);
            var songTwo = playlistSongController.GetPlaylistSongFromIndex(playlistID, indexTwo);

            songOne.Index = indexTwo;
            songTwo.Index = indexOne;

            playlistSongController.UpdatePlaylistSong(songOne);
            playlistSongController.UpdatePlaylistSong(songTwo);

            ((PlaylistDataContext)mainGrid.DataContext).OnPropertyChanged("");
        }

        private static void FinishMoving(PlaylistSong selectedSong, PlaylistSong replacedSong, PlaylistSongController playlistSongController, PlaylistDataContext dataContext)
        {
            playlistSongController.UpdatePlaylistSong(selectedSong);
            playlistSongController.UpdatePlaylistSong(replacedSong);

            dataContext.OnPropertyChanged("");
        }
    }
}
