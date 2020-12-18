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

            playlistController.CreateItem(new Playlist
            {
                Name = $"Playlist {playlistController.GetActivePlaylists(UserController.CurrentUser).Count + 1}",
                CreationDate = DateTime.Now,
                UserID = UserController.CurrentUser.ID
            });
            PlaylistMenuDataContext.Instance.OnPropertyChanged("");
        }

        private void PlaylistsRow_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MenuItemRoutedEvent?.Invoke(this, new MenuItemRoutedEventArgs
            {
                ScreenName = ScreenNames.PlaylistScreen,
                Playlist = (Playlist)((DataGridRow)sender).Item
            });
        }
    }
}