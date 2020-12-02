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

        public double Volume => AudioPlayer.Instance.WaveOutDevice.Volume;
        public double MaxVolume => AudioPlayer.Instance.MaxVolume;
        public double TotalTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.TotalTimeSong;
        public double CurrentTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong;
        public string TotalTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.TotalTimeSong).ToString("m':'ss");
        public string CurrentTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong).ToString("m':'ss");
        
        public Role CurrentUserRole => UserController.CurrentUser == null ? new Role() : RoleController.Create(new DatabaseContext()).GetItem(UserController.CurrentUser.RoleID);

        public User CurrentUser { get; set; }
        public bool IsArtist => CurrentUser != null && CurrentUser.RoleID == 2;

        private Timer _timer;

        private DataContext()
        {
            Instance = this;
            _timer = new Timer {Interval = 10};
            _timer.Elapsed += OnTimedEvent;
            _timer.Elapsed += CoinsController.Instance.EarnCoins;
            _timer.Start();
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
