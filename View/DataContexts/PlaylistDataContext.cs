using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.DbModels;
using Model.Annotations;
using Model.Database.Contexts;
using Controller;
using System;

namespace View.DataContexts
{
    public class PlaylistDataContext : INotifyPropertyChanged
    {
        private static PlaylistDataContext instance;
        public static PlaylistDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new PlaylistDataContext();
                return instance;
            }
        }

        public Playlist Playlist => Soundify.MainWindow.CurrentPlayList;

        public event PropertyChangedEventHandler PropertyChanged;

        private PlaylistDataContext()
        {
            AudioPlayer.Instance.NextSong += OnNextSong;
        }

        public void OnNextSong(object sender, EventArgs e)
        {
            OnPropertyChanged("");
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}