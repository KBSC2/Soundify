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

        public double TotalTime => Data.CurrentSong.TotalTimeSong;
        public double CurrentTime => Data.CurrentSong.CurrentTimeSong;
       
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
