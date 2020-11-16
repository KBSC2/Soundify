using System.Windows;
using System.Windows.Controls;
﻿using NAudio.Wave;
using Controller;
using Controller.DbControllers;
using Model;
using Model.Data;

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
            AudioPlayer.Initialize();
            AudioPlayer.PlaySong(new SongAudioFile("dansenaandegracht.mp3"));

            InitializeComponent();

            Context = new DatabaseContext();
            SongController = new SongController(Context, Context.Songs);
            PlaylistController = new PlaylistController(Context, Context.Playlists);
            PlaylistSongController = new PlaylistSongController(Context, Context.Playlists, Context.Songs);

            SetScreen(ScreenNames.HomeScreen);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            SetScreen(ScreenNames.HomeScreen);
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            SetScreen(ScreenNames.PlaylistScreen);
        }

        private void QueueButton_Click(object sender, RoutedEventArgs e)
        {
            SetScreen(ScreenNames.QueueScreen);
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

        public enum ScreenNames
        {
            HomeScreen,
            PlaylistScreen,
            QueueScreen
        }

        public void SetScreen(ScreenNames screenName, object dataContext = null)
        {
            this.DataContext = dataContext ?? this.DataContext;
            MainContent.ContentTemplate = FindResource(screenName.ToString()) as DataTemplate;
        }
    }
}
