using System.Windows;
using Controller;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using View.DataContexts;
using Model.EventArgs;
using Soundify;
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
            playlistController = PlaylistController.Create(DatabaseContext.Instance);
            playlistSongController = PlaylistSongController.Create(DatabaseContext.Instance);
        }

        private void Play_Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.PlayPlaylist(MainWindow.CurrentPlayList);
            SongListDataContext.Instance.OnPropertyChanged("SongInfoList");
        }

        private void RemovePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlist = playlistController.GetItem((int)((Button)sender).Tag);

            var removeConfirm = MessageBox.Show($"Are you sure you want to delete {playlist.Name}?", $"Remove {playlist.Name}", MessageBoxButton.YesNo);
            switch (removeConfirm)
            {
                case MessageBoxResult.Yes:
                    playlistController.DeactivatePlaylist(playlist);
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

            playlistSongController.SwapSongs(MainWindow.CurrentPlayList, selectedSongInfo.Index, selectedSongInfo.Index - 1);
            SongListDataContext.Instance.OnPropertyChanged("SongInfoList");
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;
            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index + 1 >= listView.Items.Count) return;
            
            playlistSongController.SwapSongs(MainWindow.CurrentPlayList, selectedSongInfo.Index, selectedSongInfo.Index + 1);
            SongListDataContext.Instance.OnPropertyChanged("SongInfoList");
        }
    }
}
