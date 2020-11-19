using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Controller;
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

        private void Play_Playlist_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.PlayPlaylist(Soundify.MainWindow.CurrentPlayList);
        }
    }
}