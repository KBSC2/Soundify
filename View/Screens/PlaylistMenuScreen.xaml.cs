using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller.DbControllers;
using Model;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using Soundify;
using View.DataContexts;

namespace View.Screens
{
    partial class PlaylistMenuScreen : ResourceDictionary
    {
        private bool rights = false;
        public PlaylistMenuScreen()
        {
            this.InitializeComponent();
        }

        private void CreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (rights)
            {
                var playlistController = new PlaylistController(new DatabaseContext());

                var playlist = new Playlist
                {
                    Name = $"Playlist {playlistController.GetActivePlaylists(DataContext.Instance.CurrentUser.ID).Count + 1}",
                    CreationDate = DateTime.Now,
                    UserID = DataContext.Instance.CurrentUser.ID
                };

                playlistController.CreateItem(playlist);
                PlaylistMenuDataContext.Instance.OnPropertyChanged("");
            }
            else
            {
                PopUpNoRights NoPlaylist= new PopUpNoRights() { Owner = MainWindow.InstanceMainWindow };
                NoPlaylist.ShowDialog();
            }
            
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