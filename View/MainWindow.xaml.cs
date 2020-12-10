using Controller;
using Controller.DbControllers;
using Model;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Model.Enums;
using View;
using View.Components;
using View.DataContexts;
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
        public static Playlist CurrentPlayList { get; internal set; }

        public static MainWindow InstanceMainWindow
        {
            get
            {
                if (instanceMainWindow == null) instanceMainWindow = new MainWindow();
                return instanceMainWindow;
            }
        }

        public static LoginScreen InstanceLoginScreen
        {
            get
            {
                if (instanceLoginScreen == null) instanceLoginScreen = new LoginScreen();
                return instanceLoginScreen;
            }
        }

        private static MainWindow instanceMainWindow;
        private static LoginScreen instanceLoginScreen;

        public MainWindow()
        {
            AudioPlayer.Create(new DatabaseContext());

            AudioPlayer.Instance.NextSong += PlaylistScreen.Instance.OnNextSong;
            instanceMainWindow = this;

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

            if (UserController.CurrentUser == null)
            {
                InstanceLoginScreen.Show();
                InstanceMainWindow.Hide();
            }
            
            PermissionController.NoRightsEvent += ShowNoRights;
        }

        /**
         * Find all the objects on specified window
         *
         * @param depObj The window you want the objects to be selected from
         *
         * @return IEnumerable<T> : Returns the object from type T
         */
        public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
                {
                    if (rawChild is DependencyObject)
                    {
                        DependencyObject child = (DependencyObject) rawChild;
                        if (child is T)
                        {
                            yield return (T) child;
                        }

                        foreach (T childOfChild in FindLogicalChildren<T>(child))
                            yield return childOfChild;
                    }
                }
            }
        }

        /**
         * Updates all the permission buttons on MainWindow
         *
         * @return void
         */
        public void UpdateButtons()
        {
            foreach (PermissionButton button in FindLogicalChildren<PermissionButton>(MainWindow.InstanceMainWindow))
                button.UpdateButton();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.Instance.CurrentSongFile == null)
            {
                AudioPlayer.Instance.Next();
                QueueDataContext.Instance.OnPropertyChanged();
            }

            if (AudioPlayer.Instance.WaveOutDevice.PlaybackState == PlaybackState.Paused || AudioPlayer.Instance.WaveOutDevice.PlaybackState == PlaybackState.Stopped)
            {
                if (AudioPlayer.Instance.Queue.Count > 0)
                {
                    AudioPlayer.Instance.WaveOutDevice.Play();
                    Play.Content = "=";
                }
            }
            else
            {
                AudioPlayer.Instance.WaveOutDevice.Pause();
                Play.Content = ">";
            }
        }

        private void Duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            AudioPlayer.Instance.CurrentSongFile.AudioFile.Skip((int)(slider.Value - AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong));
        }

        public void SetScreen(ScreenNames screenName)
        {
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
            AudioPlayer.Instance.WaveOutDevice.Volume = (float)slider.Value;

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
            AudioPlayer.Instance.Loop(CurrentPlayList);
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
                
                SongListDataContext.Instance.SongList = SongController.Create(new DatabaseContext()).SearchSongsOnString(text.Split(" ").ToList());
                SongListDataContext.Instance.ScreenName = ScreenNames.SearchScreen;

                SongListDataContext.Instance.OnPropertyChanged();

                SetScreen(ScreenNames.SearchScreen);
                SearchDataContext.Instance.OnPropertyChanged("");
            }   
        }

        public void ShowNoRights(object sender, NoRightsEventArgs e)
        {
            PopUpNoRights popUpNoRights = new PopUpNoRights(e.Permission);
            popUpNoRights.ShowDialog();
        }

        private void WindowClosing(object sender, EventArgs e)
        {
            SSHController.Instance.CloseSSHTunnel();
            Application.Current.Shutdown();
        }
    }
}