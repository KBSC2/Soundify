using Controller;
using Controller.DbControllers;
using Model;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using NAudio.Wave;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using View;
using View.DataContexts;

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
            var configLocation = Assembly.GetEntryAssembly().Location;

            AudioPlayer.Initialize();
            _instanceMainWindow = this;

            InitializeComponent();
            SSHController.Instance.OpenSSHTunnel();

            Context = new DatabaseContext();
            new PlaylistController(Context).DeletePlaylistOnDateStamp();

            SetScreen(ScreenNames.HomeScreen);
            MenuItemRoutedEvent += OnMenuItemRoutedEvent;

            if (View.DataContexts.DataContext.Instance.CurrentUser == null)
            {
                InstanceLoginScreen.Show();
                InstanceMainWindow.Hide();
            }
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.CurrentSong == null)
                AudioPlayer.Next();

            if (AudioPlayer.WaveOutDevice.PlaybackState == PlaybackState.Paused || AudioPlayer.WaveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                if (AudioPlayer.SongQueue.Count > 0)
                {
                    AudioPlayer.WaveOutDevice.Play();
                    Play.Content = "=";
                }
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
                View.DataContexts.DataContext.Instance.CurrentUser = null;
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

        public void SetSearchTerms(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textBox = (TextBox) sender;
                var text = textBox.Text;
                SetScreen(ScreenNames.SearchScreen);
                SearchDataContext.Instance.SearchTerms = text.Split(" ").ToList();
                SearchDataContext.Instance.OnPropertyChanged("");
            }   
        }
    }
}