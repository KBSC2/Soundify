using Soundify;
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
using System.Windows.Shapes;
using Controller.DbControllers;
using Model;
using Model.Data;

namespace View
{
    /// <summary>
    /// Interaction logic for Playlist.xaml
    /// </summary>
    public partial class Playlist : Window
    {
        public Playlist(int id)
        {
            InitializeComponent();
            
            DatabaseContext databaseContext = new DatabaseContext();

            PlaylistController playlistController = new PlaylistController(databaseContext, databaseContext.Playlists);

            PlaylistDataContext dataContext = (PlaylistDataContext)DataContext;
            dataContext.Playlist = playlistController.GetItem(id);
            dataContext.AddPlaylistsToMenu();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            this.Close();
            win2.Show();
        }

        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            PlaylistMenu win3 = new PlaylistMenu();
            this.Close();
            win3.Show();
        }

        private void QueueButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
