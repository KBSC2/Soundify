using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Controller;

namespace View.DataContexts
{
    class SongInfoDataContext : INotifyPropertyChanged
    {
        public Song Song { get; set; }
        public string PathToImage => Song.PathToImage == null ? FileCache.Instance.GetFile("images/gangnamstyle.png") : FileCache.Instance.GetFile(Song.PathToImage);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
