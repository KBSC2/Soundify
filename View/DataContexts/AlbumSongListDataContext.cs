using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model.DbModels;
using Model.Annotations;
using Model.Database.Contexts;
using Controller;
using System;

namespace View.DataContexts
{
    public class AlbumSongListDataContext : INotifyPropertyChanged
    {
        private static AlbumSongListDataContext instance;
        public static AlbumSongListDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new AlbumSongListDataContext();
                return instance;
            }
        }

        public Album Album { get; set; }    

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}