using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Controller.DbControllers;
using Model;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace View.DataContexts
{
    public class SongListDataContext : INotifyPropertyChanged
    {
        private static SongListDataContext instance;
        public static SongListDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new SongListDataContext();
                return instance;
            }
        }

        public bool IsPlaylistScreen => SongListDataContext.Instance.ScreenName.Equals(ScreenNames.PlaylistScreen);

        public ScreenNames ScreenName { get; set; }


        public List<Song> SongList { get; set; }

        public List<SongInfo> SongInfoList => SongInfo.ConvertSongListToSongInfo(SongList);

        public List<Playlist> AllPlaylists => PlaylistController.Create(new DatabaseContext()).GetActivePlaylists(UserController.CurrentUser.ID);

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
