using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for SearchScreen.xaml
    /// </summary>
    public partial class SearchScreen : ResourceDictionary
    {
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
