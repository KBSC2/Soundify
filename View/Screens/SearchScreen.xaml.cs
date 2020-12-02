using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

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

            var playlistSongController = PlaylistSongController.Create(new DatabaseContext());
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);
        }
    }
}
