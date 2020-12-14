using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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

        public List<string> SongListSearchTerms { get; set; } = new List<string>(){""};

        public bool IsSongListScreen => Instance.ScreenName.Equals(ScreenNames.SongListScreen);

        public void UpdateSongInfoList()
        {
            var artistController = ArtistController.Create(new DatabaseContext());
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
                        .Where(s => s.Artist.Equals(
                            artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID))).ToList();
                    break;
                case ScreenNames.SongListScreen:
                    if (DataContext.Instance.IsAdmin != null && (bool)DataContext.Instance.IsAdmin)
                    {
                        songlist = SongController.Create(new DatabaseContext())
                            .SearchSongsOnString(SongListSearchTerms.ToList());
                    }
                    else if (DataContext.Instance.IsArtist != null && (bool) DataContext.Instance.IsArtist)
                    {
                        songlist = SongController.Create(new DatabaseContext())
                            .SearchSongsOnString(SongListSearchTerms.ToList())
                            .Where(s => s.Artist.Equals(
                                artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID))).ToList();
                    }
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