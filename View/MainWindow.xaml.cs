using System;
using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;
using Controller;
using Controller.DbControllers;
using Model;
using Model.Data;
using Model.DbModels;
using Model.EventArgs;
using View;
using View.Screens;

namespace Soundify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Events
        public static EventHandler<MenuItemRoutedEventArgs> MenuItemRoutedEvent;
        #endregion

        public DatabaseContext Context { get; set; }
        public PlaylistSongController PlaylistSongController { get; set; }
        public SongController SongController { get; set; }
        public PlaylistController PlaylistController { get; set; }
        public static Playlist CurrentPlayList { get; internal set; }

        public static MainWindow InstanceMainWindow
        {
            get
            {
                if (_instanceMainWindow == null) _instanceMainWindow = new MainWindow();
                return _instanceMainWindow;
            }
        }

        public static LoginScreen InstanceLoginScreen
        {
            get
            {
                if (_instanceLoginScreen == null) _instanceLoginScreen = new LoginScreen();
                return _instanceLoginScreen;
            }
        }

        public static MainWindow _instanceMainWindow;
        public static LoginScreen _instanceLoginScreen;

        public MainWindow()
        {
            AudioPlayer.Initialize();
            AudioPlayer.AddSong(new SongAudioFile("dansenaandegracht.mp3"));
            AudioPlayer.AddSong(new SongAudioFile("untrago.mp3"));
            _instanceMainWindow = this;

            InitializeComponent();
            SSHController.Instance.OpenSSHTunnel();

            Context = new DatabaseContext();
            new PlaylistController(Context).DeletePlaylistOnDateStamp();

            SetScreen(ScreenNames.HomeScreen);
            MenuItemRoutedEvent += OnMenuItemRoutedEvent;

            if (View.DataContext.Instance.CurrentUser == null)
            {
                InstanceLoginScreen.Show();
                InstanceMainWindow.Hide();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.CurrentSong == null)
                AudioPlayer.Next();

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

        public void SetScreen(ScreenNames screenName)
        {
            if (screenName == ScreenNames.Logout)
            {
                View.DataContext.Instance.CurrentUser = null;
                var login = new LoginScreen();
                login.Show();
                login.Focus();
                this.Hide();
                return;
            }
            MainContent.ContentTemplate = FindResource(screenName.ToString()) as DataTemplate;
        }
        public void SetScreen(ScreenNames screenName, Playlist playlist)
        {
            MainContent.ContentTemplate = FindResource(screenName.ToString()) as DataTemplate;
            CurrentPlayList = playlist;
        }

        public void OnMenuItemRoutedEvent(object sender, MenuItemRoutedEventArgs args)
        {
            if (args.ScreenName == ScreenNames.PlaylistScreen) SetScreen(args.ScreenName, args.Playlist);
            else SetScreen(args.ScreenName);
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            AudioPlayer.WaveOutDevice.Volume = (float)slider.Value;

        }

        private void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Prev();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Next();
        }

        private void Loop_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Loop();
        }

        private void Shuffle_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Shuffle();
        }
    }

}