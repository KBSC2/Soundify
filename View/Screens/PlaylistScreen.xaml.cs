using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
using Microsoft.Extensions.Options;
using Model.Data;
using Model.DbModels;
using View.DataContexts;

namespace View.Screens
{
    partial class PlaylistScreen : ResourceDictionary
    {
        public PlaylistScreen()
        {
            this.InitializeComponent();
        }
        private void ListViewItem_RightClick(object sender, RoutedEventArgs e)
        {
            /*var song = (Song)(((MenuItem)sender).Tag);
            SongInfoScreen songInfoScreen = new SongInfoScreen(song);
            songInfoScreen.Show();*/
            MenuItem menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                ContextMenu parentContextMenu = menuItem.CommandParameter as ContextMenu;
                if (parentContextMenu != null)
                {
                    ListViewItem listViewItem = parentContextMenu.PlacementTarget as ListViewItem;
                    var songInfo = (SongInfo)(listViewItem.Content);
                }
            }
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

            if (selectedSongInfo == null || selectedSongInfo.Index == 1) return;

            var playlistID = Soundify.MainWindow.CurrentPlayList.ID;

            var playlistSongController = new PlaylistSongController(new DatabaseContext());

            var selectedSong = playlistSongController.GetPlaylistSong(playlistID, selectedSongInfo.Song.ID);
            selectedSong.Index = selectedSongInfo.Index - 1;

            var replacedSong = playlistSongController.GetPlaylistSongFromIndex(playlistID, selectedSong.Index);
            replacedSong.Index = selectedSongInfo.Index;

            FinishMoving(selectedSong, replacedSong, playlistSongController, (PlaylistDataContext)mainGrid.DataContext);
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var mainGrid = (Grid)((Button)sender).Tag;

            var listView = (ListView) mainGrid.FindName("SongList");
            var selectedSongInfo = (SongInfo)listView?.SelectedItem;

            if (selectedSongInfo == null || selectedSongInfo.Index >= listView.Items.Count) return;

            var playlistID = Soundify.MainWindow.CurrentPlayList.ID;

            var playlistSongController = new PlaylistSongController(new DatabaseContext());

            var selectedSong = playlistSongController.GetPlaylistSong(playlistID, selectedSongInfo.Song.ID);
            selectedSong.Index = selectedSongInfo.Index + 1;

            var replacedSong = playlistSongController.GetPlaylistSongFromIndex(playlistID, selectedSong.Index);
            replacedSong.Index = selectedSongInfo.Index;

            FinishMoving(selectedSong, replacedSong, playlistSongController, (PlaylistDataContext)mainGrid.DataContext);
        }

        private static void FinishMoving(PlaylistSong selectedSong, PlaylistSong replacedSong, PlaylistSongController playlistSongController, PlaylistDataContext dataContext)
        {
            playlistSongController.UpdatePlaylistSong(selectedSong);
            playlistSongController.UpdatePlaylistSong(replacedSong);

            dataContext.OnPropertyChanged("");
        }
    }
}
