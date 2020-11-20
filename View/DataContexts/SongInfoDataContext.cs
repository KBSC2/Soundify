using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace View.DataContexts
{
    class SongInfoDataContext : INotifyPropertyChanged
    {
        public Song Song { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
