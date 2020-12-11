using Controller;
using System;
using System.ComponentModel;
using System.Timers;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class DataContext : INotifyPropertyChanged
    {
        private static DataContext _instance;
        public static DataContext Instance
        {
            get
            {
                if (_instance == null)
                    new DataContext();
                return _instance;
            }
            set => _instance = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Song CurrentSong => AudioPlayer.Instance.CurrentSong;
        public string PathToImage => CurrentSong == null ? "" : CurrentSong.PathToImage == null ? "" : FileCache.Instance.GetFile(CurrentSong.PathToImage);
        public double Volume => AudioPlayer.Instance.WaveOutDevice.Volume;
        public double MaxVolume => AudioPlayer.Instance.MaxVolume;
        public double TotalTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.TotalTimeSong;
        public double CurrentTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong;
        public string TotalTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.TotalTimeSong).ToString("m':'ss");
        public string CurrentTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong).ToString("m':'ss");

        public User CurrentUser => UserController.CurrentUser == null ? null : UserController.Create(new DatabaseContext())
                .GetItem(UserController.CurrentUser.ID);
        public int? CurrentUserCoins => CurrentUser?.Coins;
        public Role CurrentUserRole => UserController.CurrentUser == null ? null : RoleController.Create(new DatabaseContext()).GetItem(UserController.CurrentUser.RoleID);
        public bool? IsAdmin => CurrentUser?.RoleID.Equals(3);
        public bool? IsArtist => CurrentUser?.RoleID.Equals(2);

        public string DisplayName => IsArtist.GetValueOrDefault(false)
            ? ArtistController.Create(new DatabaseContext()).GetArtistFromUserId(UserController.CurrentUser?.ID)?.ArtistName 
            : UserController.CurrentUser?.Username;

        private Timer timerSlider;
        private Timer timer;

        private DataContext()
        {
            Instance = this;
            timer = new Timer {Interval = 1000};
            timer.Elapsed += OnTimedEvent;
            timer.Elapsed += CoinsController.Instance.EarnCoins;
            timer.Start();

            timerSlider = new Timer { Interval = 10 };
            timerSlider.Elapsed += OnTimedEventSlider;
            timerSlider.Start();
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnTimedEventSlider(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentTime"));
        }
    }
}
