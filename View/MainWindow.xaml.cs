using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;
using Controller;
using Controller.DbControllers;
using Model;
using Model.Data;
using Model.DbModels;
using View;
using Playlist = View.Playlist;
using Renci.SshNet;

 namespace Soundify
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool CreateSSHTunnel = true;

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
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            this.Close();
            win2.Show();
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            PlaylistMenu win3 = new PlaylistMenu();
            this.Close();
            win3.Show();

            OpenSSHTunnel();
        }

        public void OpenSSHTunnel()
        {
            if (!CreateSSHTunnel)
                return;

            using (var client = new SshClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
            {
                client.Connect();

                var port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);
                client.AddForwardedPort(port);

                port.Start();
            }
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