using Controller;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class DataContext : INotifyPropertyChanged
    {
        private static DataContext instance;
        public static DataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataContext();
                return instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Volume => AudioPlayer.Instance.WaveOutDevice.Volume;
        public double MaxVolume => AudioPlayer.Instance.MaxVolume;
        public double TotalTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.TotalTimeSong;
        public double CurrentTime => AudioPlayer.Instance.CurrentSongFile == null ? 0 : AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong;
        public string TotalTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.TotalTimeSong).ToString("m':'ss");
        public string CurrentTimeLabel => AudioPlayer.Instance.CurrentSongFile == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSongFile.CurrentTimeSong).ToString("m':'ss");

        public User CurrentUser => UserController.CurrentUser;
        public int? CurrentUserCoins => CurrentUser?.Coins;
        public bool? IsAdmin => CurrentUser?.RoleID.Equals(3);
        public bool? IsArtist => CurrentUser?.RoleID.Equals(2);
        public Role CurrentUserRole => CurrentUser?.Role;

        private DataContext()
        {
            Timer timer = new Timer {Interval = 1000};
            timer.Elapsed += OnTimedEvent;
            timer.Elapsed += CoinsController.Instance.EarnCoins;
            timer.Start();

            Timer timerSlider = new Timer { Interval = 10 };
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
