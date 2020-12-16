using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            AudioPlayer.Create(DatabaseContext.Instance);

            instanceMainWindow = this;

            if (!Directory.Exists(Path.GetTempPath() + "Soundify"))
                Directory.CreateDirectory(Path.GetTempPath() + "Soundify");

            foreach (PathDirectories path in Enum.GetValues(typeof(PathDirectories)))
            {
                if (!Directory.Exists(Path.GetTempPath() + "Soundify/" + path.ToString().ToLower()))
                    Directory.CreateDirectory(Path.GetTempPath() + "Soundify/" + path.ToString().ToLower());
            }

            SSHController.Instance.OpenSSHTunnel();
            InitializeComponent();

            Context = DatabaseContext.Instance;
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
            foreach (object c in LogicalTreeHelper.GetChildren(depObj))
            {
                if (c is DependencyObject child) {
                    if (child is T)
                        yield return (T) child;

                    foreach (T childChild in FindLogicalChildren<T>(child))
                        yield return childChild;
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
            SynchronizationContext.Current.Post(o =>
            {
                ((List<PermissionButton>)o).ForEach(x => x.UpdateButton());
            }, View.DataContexts.DataContext.PermissionButtons);
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.Instance.CurrentSongFile == null)
            {
                AudioPlayer.Instance.Next();
                QueueDataContext.Instance.OnPropertyChanged();
            }

            AudioPlayer.Instance.PlayPause();
        }

        private void Duration_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider slider && AudioPlayer.Instance.CurrentSongFile != null)
            {
                AudioPlayer.Instance.CurrentSongFile.AudioFile.Skip(
                    (int) (slider.Value - AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong));
            }
        }

        public void SetScreen(ScreenNames screenName)
        {
            MainContent.ContentTemplate = FindResource(screenName.ToString()) as DataTemplate;
            SongListDataContext.Instance.ScreenName = screenName;
            SongListDataContext.Instance.OnPropertyChanged();
        }

        public void SetScreen(ScreenNames screenName, Playlist playlist)
        {
            CurrentPlayList = playlist;
            SetScreen(screenName);
        }

        public void OnMenuItemRoutedEvent(object sender, MenuItemRoutedEventArgs args)
        {
            if (args.ScreenName == ScreenNames.PlaylistScreen) SetScreen(args.ScreenName, args.Playlist);
            else SetScreen(args.ScreenName);
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            AudioPlayer.Instance.WaveOutDevice.Volume = (float) slider.Value;
        }

        private void Prev_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.Prev();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.NextButton();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Loop_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.Loop();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        private void Shuffle_Button_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Instance.Shuffle();
            QueueDataContext.Instance.OnPropertyChanged();
        }

        public void SetSearchTerms(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var textBox = (TextBox) sender;
                var text = textBox.Text;

                if (SongListDataContext.Instance.IsSongListScreen)
                {
                    SongListDataContext.Instance.SongListSearchTerms = text.Split(" ").ToList();
                    SongListDataContext.Instance.OnPropertyChanged();
                }
                else
                {
                    SearchDataContext.Instance.SearchTerms = text.Split(" ").ToList();
                    SetScreen(ScreenNames.SearchScreen);
                }
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

        private void Volume_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AudioPlayer.Instance.WaveOutDevice.Volume == 0.0)
                AudioPlayer.Instance.WaveOutDevice.Volume = (float)(AudioPlayer.Instance.MaxVolume / 2);
            else
                AudioPlayer.Instance.WaveOutDevice.Volume = 0.0f;
        }       
    }
}