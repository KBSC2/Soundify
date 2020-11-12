using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation.Language;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;

namespace View
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TotalTimeLabel => Math.Floor(Data.CurrentSong.TotalTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(Data.CurrentSong.TotalTimeSong % 60));
        public string CurrentTimeLabel => Math.Floor(Data.CurrentSong.CurrentTimeSong / 60) + ":" + string.Format("{0:00}",Math.Floor(Data.CurrentSong.CurrentTimeSong % 60));

        private Timer _timer;

        public DataContext()
        {
            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        public void OnTimedEvent(Object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
