using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : ResourceDictionary
    {
        private SearchDataContext searchDataContext;
        public SearchScreen()
        {
            InitializeComponent();

        }

        
        private void MenuItem_LeftClick(object sender, MouseButtonEventArgs e)
        {
            var playlist = ((Playlist)((MenuItem)sender).DataContext);
            var song = ((SongInfo)((MenuItem)((MenuItem)sender).Tag).DataContext).Song;

            var playlistSongController = new PlaylistSongController(new DatabaseContext());
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);
        }
    }
}
