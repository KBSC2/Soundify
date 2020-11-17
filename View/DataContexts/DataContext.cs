using Controller;
using System;
using System.ComponentModel;
using System.Timers;

namespace View
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Volume => AudioPlayer.CurrentSong == null ? 0 : AudioPlayer.CurrentSong.AudioFile.Volume;
        public double TotalTime => AudioPlayer.CurrentSong == null ? 0 : AudioPlayer.CurrentSong.TotalTimeSong;
        public double CurrentTime => AudioPlayer.CurrentSong == null ? 0 : AudioPlayer.CurrentSong.CurrentTimeSong;
        public string TotalTimeLabel => AudioPlayer.CurrentSong == null ? 
            "" : Math.Floor(AudioPlayer.CurrentSong.TotalTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(AudioPlayer.CurrentSong.TotalTimeSong % 60));
        public string CurrentTimeLabel => AudioPlayer.CurrentSong == null ? 
            "" : Math.Floor(AudioPlayer.CurrentSong.CurrentTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(AudioPlayer.CurrentSong.CurrentTimeSong % 60));

        private Timer _timer;

        public DataContext()
        {
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
