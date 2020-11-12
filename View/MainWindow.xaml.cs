using System.Windows;
 using System.Windows.Controls;
 using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
﻿using NAudio.Wave;
using Controller;
using Model;

namespace Soundify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DatabaseContext Context { get; set; }
        public PlaylistSongController PlaylistSongController { get; set; }
        public SongController SongController { get; set; }
        public PlaylistController PlaylistController { get; set; }

        public MainWindow()
        {
            
            Data.Initialize();
            Data.PlaySong(new Song(new AudioFileReader("dansenaandegracht.mp3")));
            InitializeComponent();

            Context = new DatabaseContext();
            SongController = new SongController(Context, Context.Songs);
            PlaylistController = new PlaylistController(Context, Context.Playlists);
            PlaylistSongController = new PlaylistSongController(Context, Context.Playlists, Context.Songs);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            this.Close();
            win2.Show();
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            //PlaylistMenu win3 = new PlaylistMenu();
            // temporarily until playlist menu is implemented
            var win3 = new View.Playlist();
            this.Close();
            win3.Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Data.WaveOutDevice.PlaybackState == PlaybackState.Paused || Data.WaveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                Data.WaveOutDevice.Play();
                Play.Content = "=";
            }
            else
            {
                Data.WaveOutDevice.Pause();
                Play.Content = ">";
            }
        }

        private void Duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            Data.CurrentSong.AudioFile.Skip((int)(slider.Value - Data.CurrentSong.CurrentTimeSong));
        }
    }
}
