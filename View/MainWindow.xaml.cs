﻿using System.Windows;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using Playlist = View.Playlist;
﻿using NAudio.Wave;
using NAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using Controller;
using Model;
using NAudio.Wave;
using System.Windows;
using System.Windows.Controls;

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
            new FileCache();
            AudioPlayer.Initialize();
            AudioPlayer.PlaySong(new SongAudioFile("dansenaandegracht.mp3"));
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
            var win3 = new Playlist();
            this.Close();
            win3.Show();

            FileTransfer.DownloadFile("test.txt");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.WaveOutDevice.PlaybackState == PlaybackState.Paused || AudioPlayer.WaveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                AudioPlayer.WaveOutDevice.Play();
                Play.Content = "=";
            }
            else
            {
                AudioPlayer.WaveOutDevice.Pause();
                Play.Content = ">";
            }
        }

        private void Duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            AudioPlayer.CurrentSong.AudioFile.Skip((int)(slider.Value - AudioPlayer.CurrentSong.CurrentTimeSong));
        }
    }
}
