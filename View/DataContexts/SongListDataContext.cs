using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Controller.DbControllers;
using Model;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Soundify;

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

        public ScreenNames ScreenName { get; set; }

        public void UpdateSongInfoList()
        {
            List<Song> songlist = new List<Song>();
            switch (instance.ScreenName)
            {
                case ScreenNames.PlaylistScreen:
                    songlist = PlaylistSongController.Create(new DatabaseContext())
                        .GetSongsFromPlaylist(Soundify.MainWindow.CurrentPlayList.ID).Select(ps => ps.Song).ToList();
                    break;
                case ScreenNames.SearchScreen:
                    songlist = SongController.Create(new DatabaseContext())
                        .SearchSongsOnString(SearchDataContext.Instance.SearchTerms.ToList());
                    break;
                case ScreenNames.ArtistScreen:
                    songlist = SongController.Create(new DatabaseContext()).GetList()
                        .Where(s => s.Artist == ArtistController.Create(new DatabaseContext()).GetArtistIdFromUserId(UserController.CurrentUser.ID)).ToList();
                    break;
            }

            SongInfoList = SongInfo.ConvertSongListToSongInfo(songlist);
        }

        public List<SongInfo> SongInfoList { get; set; }

        public List<Playlist> AllPlaylists => PlaylistController.Create(new DatabaseContext())
            .GetActivePlaylists(UserController.CurrentUser.ID);

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            UpdateSongInfoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}