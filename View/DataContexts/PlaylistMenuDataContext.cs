using System;
using System.Collections.Generic;
using System.ComponentModel;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;

namespace View.DataContexts
{
    public class PlaylistMenuDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Playlist> PlaylistsSource => new PlaylistController(new DatabaseContext()).GetActivePlaylists();

        public PlaylistMenuDataContext()
        {
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}