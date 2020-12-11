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

        public void OnNextSong(object sender, EventArgs e)
        {
            PlaylistDataContext.Instance.OnPropertyChanged("");
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
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index + 1 >= listView.Items.Count) return;
            
            playlistSongController.SwapSongs(selectedSongInfo.Index, selectedSongInfo.Index + 1, MainWindow.CurrentPlayList.ID);
            PlaylistDataContext.Instance.OnPropertyChanged("");
        }

    }
}
