using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
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

        private void RemovePlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            var playlistID = (int)((Button) sender).Tag;

            var playlistController = new PlaylistController(new DatabaseContext());
            var playlistName = playlistController.GetItem(playlistID);

            var removeConfirm = MessageBox.Show($"Are you sure you want to delete {playlistName}?", $"Remove {playlistName}", MessageBoxButton.YesNoCancel);
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
    }
}