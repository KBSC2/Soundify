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

namespace View
{
    /// <summary>
    /// Interaction logic for PlaylistMenu.xaml
    /// </summary>
    public partial class PlaylistMenu : Window
    {

        private int _numberOfRows = -1;
        public PlaylistMenu()
        {
            InitializeComponent();

            // loop through all the playlists in the database
            // for each playlist, execute the following method with different row and col values
            AddPlaylistToGrid(0, 0);
            AddPlaylistToGrid(0, 1);
            AddPlaylistToGrid(0, 2);
            AddPlaylistToGrid(1, 0);
            AddPlaylistToGrid(1, 1);
            AddPlaylistToGrid(1, 2);
            AddPlaylistToGrid(2, 0);
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

        private void AddPlaylistToGrid(int row, int col)
        {
            // if a new row starts, add a new row to the grid
            if (_numberOfRows < row)
            {
                PlaylistOverview.RowDefinitions.Add(new RowDefinition());
                PlaylistOverview.Height += 100;
                _numberOfRows++;
            }

            // add a maximum of 3 columns to the grid
            if (PlaylistOverview.ColumnDefinitions.Count < 3)
            {
                PlaylistOverview.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var innerGrid = new Grid {MaxHeight = 100};
            
            innerGrid.RowDefinitions.Add(new RowDefinition());
            innerGrid.RowDefinitions.Add(new RowDefinition());
            innerGrid.RowDefinitions.Add(new RowDefinition());
            innerGrid.RowDefinitions.Add(new RowDefinition());

            innerGrid.Background = Brushes.Transparent;
            innerGrid.MouseLeftButtonDown += (sen, evg) =>
            {
                var playlistWindow = new Playlist();
                this.Close();
                playlistWindow.Show();
            };

            var coverImage = new Image { Source = null };
            Grid.SetRow(coverImage, 0);

            var nameLabel = new Label { Content = "Playlist naam" };
            nameLabel.Foreground = Brushes.White;
            Grid.SetRow(nameLabel, 1);

            var genreLabel = new Label { Content = "Playlist genre" };
            genreLabel.Foreground = Brushes.White;
            Grid.SetRow(genreLabel, 2);

            var creationDateLabel = new Label { Content = "Playlist creation date" };
            creationDateLabel.Foreground = Brushes.White;
            Grid.SetRow(creationDateLabel, 3);

            innerGrid.Children.Add(coverImage);
            innerGrid.Children.Add(nameLabel);
            innerGrid.Children.Add(genreLabel);
            innerGrid.Children.Add(creationDateLabel);
            Grid.SetRow(innerGrid, row);
            Grid.SetColumn(innerGrid, col);
            innerGrid.HorizontalAlignment = HorizontalAlignment.Center;

            PlaylistOverview.Children.Add(innerGrid);
        }

        private void CreateNewPlaylist_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
