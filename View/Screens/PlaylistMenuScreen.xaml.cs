using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Model.EventArgs;
using Soundify;
using View.DataContexts;

namespace View.Screens
{
    partial class PlaylistMenuScreen : ResourceDictionary
    {
        public PlaylistMenuScreen()
        {
            this.InitializeComponent();
        }

        private void CreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            var playlistController = PlaylistController.Create(DatabaseContext.Instance);

            var playlist = new Playlist
            {
                Name = $"Playlist {playlistController.GetActivePlaylists(UserController.CurrentUser).Count + 1}",
                CreationDate = DateTime.Now,
                UserID = UserController.CurrentUser.ID
            };

            playlistController.CreateItem(playlist);
            PlaylistMenuDataContext.Instance.OnPropertyChanged("");
        }

        private void PlaylistsRow_Click(object sender, MouseButtonEventArgs e)
        {
            var dataGridRow = (DataGridRow)sender;
            var selectedPlaylist = (Playlist)dataGridRow.Item;
            

            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.PlaylistScreen,
                Playlist = selectedPlaylist
            });
        }
    }
}