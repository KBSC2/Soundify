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

        public double Volume => AudioPlayer.WaveOutDevice.Volume;
        public double TotalTime => AudioPlayer.CurrentSong == null ? 0 : AudioPlayer.CurrentSong.TotalTimeSong;
        public double CurrentTime => AudioPlayer.CurrentSong == null ? 0 : AudioPlayer.CurrentSong.CurrentTimeSong;
        public string TotalTimeLabel => AudioPlayer.CurrentSong == null ? 
            "" : Math.Floor(AudioPlayer.CurrentSong.TotalTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(AudioPlayer.CurrentSong.TotalTimeSong % 60));
        public string CurrentTimeLabel => AudioPlayer.CurrentSong == null ? 
            "" : Math.Floor(AudioPlayer.CurrentSong.CurrentTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(AudioPlayer.CurrentSong.CurrentTimeSong % 60));

        public User CurrentUser { get; set; }
        public Role CurrentUserRole => CurrentUser == null ? new Role() : new RoleController(new DatabaseContext()).GetItem(CurrentUser.RoleID);
        public bool IsArtist => CurrentUser != null && CurrentUser.RoleID == 3;

        private Timer _timer;

        private DataContext()
        {
            Instance = this;
            _timer = new Timer {Interval = 10};
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
