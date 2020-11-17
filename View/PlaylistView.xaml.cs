﻿using Soundify;
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
            PlaylistSongController playlistSongController = new PlaylistSongController(databaseContext, databaseContext.Playlists, databaseContext.Songs);

            PlaylistDataContext dataContext = (PlaylistDataContext)DataContext;
            dataContext.Playlist = playlistController.GetItem(id);
            dataContext.Songs = playlistSongController.GetSongsFromPlaylist(id);
            dataContext.AddSongsToList();
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

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = playlistSongList.SelectedIndex;
            var item = playlistSongList.Items[currentIndex];
            if (currentIndex > 0)
            {
                playlistSongList.Items.RemoveAt(currentIndex);
                playlistSongList.Items.Insert(currentIndex - 1, item);
            }
        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = playlistSongList.SelectedIndex;
            var item = playlistSongList.Items[currentIndex];
            if (currentIndex < playlistSongList.Items.Count)
            {
                playlistSongList.Items.RemoveAt(currentIndex);
                playlistSongList.Items.Insert(currentIndex + 1, item);
            }
        }
    }
}
