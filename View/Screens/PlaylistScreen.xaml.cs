using System.Windows;
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
using Model.Enums;

namespace View.Screens
{
    partial class PlaylistScreen : ResourceDictionary
    {
        private PlaylistController playlistController;
        private PlaylistSongController playlistSongController;
        public static PlaylistScreen Instance { get; set; }

        public PlaylistScreen()
        {
            this.InitializeComponent();
            Instance = this;
            playlistController = PlaylistController.Create(new DatabaseContext());
            playlistSongController = PlaylistSongController.Create(new DatabaseContext());
        }

        private void Play_Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList);
        }

        private void RemovePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlistID = (int)((Button)sender).Tag;

            var playlistName = playlistController.GetItem(playlistID).Name;

            var removeConfirm = MessageBox.Show($"Are you sure you want to delete {playlistName}?", $"Remove {playlistName.ToString()}", MessageBoxButton.YesNo);
            switch (removeConfirm)
            {
                case MessageBoxResult.Yes:
                    playlistController.DeactivatePlaylist(playlistID);
                    MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs() { ScreenName = ScreenNames.PlaylistMenuScreen } );
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var selectedSongInfo = (SongInfo)((ListView)mainGrid.FindName("SongList"))?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index - 1 < 0) return;

            playlistSongController.SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index - 1, MainWindow.CurrentPlayList.ID);
            SongListDataContext.Instance.OnPropertyChanged("");
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index + 1 >= listView.Items.Count) return;
            
            playlistSongController.SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index + 1, MainWindow.CurrentPlayList.ID);
            SongListDataContext.Instance.OnPropertyChanged("");
        }
    }
}
