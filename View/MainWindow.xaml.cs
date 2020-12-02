using Controller;
using Controller.DbControllers;
using Model;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model.Enums;
using View;
using View.DataContexts;
using System.Timers;

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

        private static MainWindow _instanceMainWindow;
        private static LoginScreen _instanceLoginScreen;

        public MainWindow()
        {
            AudioPlayer.Create(new DatabaseContext());
            _instanceMainWindow = this;

            if (!Directory.Exists(Path.GetTempPath() + "Soundify"))
                Directory.CreateDirectory(Path.GetTempPath() + "Soundify");

            foreach (PathDirectories path in Enum.GetValues(typeof(PathDirectories)))
            {
                if (!Directory.Exists(Path.GetTempPath() + "Soundify/" + path.ToString().ToLower()))
                    Directory.CreateDirectory(Path.GetTempPath() + "Soundify/" + path.ToString().ToLower());
            }


            InitializeComponent();
            SSHController.Instance.OpenSSHTunnel();

            Context = new DatabaseContext();
            PlaylistController.Create(Context).DeletePlaylistOnDateStamp();

            SetScreen(ScreenNames.HomeScreen);
            MenuItemRoutedEvent += OnMenuItemRoutedEvent;

            FileCache.Instance.GetFile("images/gangnamstyle.png");
            if (UserController.CurrentUser == null)
            {
                InstanceLoginScreen.Show();
                InstanceMainWindow.Hide();
            }
        }
        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.CurrentSongFile == null)
                AudioPlayer.Instance.Next();

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
            AudioPlayer.CurrentSongFile.AudioFile.Skip((int)(slider.Value - AudioPlayer.CurrentSongFile.CurrentTimeSong));
        }

        public void SetScreen(ScreenNames screenName)
        {
            if (screenName == ScreenNames.Logout)
            {
                UserController.CurrentUser = null;
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
            AudioPlayer.Instance.Prev();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.Next();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Loop_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Looping = !AudioPlayer.Looping;
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Shuffle_Button_Click(object sender, RoutedEventArgs e)
        {
            QueueDataContext.Instance.OnPropertyChanged();
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