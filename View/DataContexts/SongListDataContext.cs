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
                    songlist = SongController.Create(new DatabaseContext()).SearchSongsOnString(SearchDataContext.Instance.SearchTerms.ToList());
                    break;
                case ScreenNames.ArtistScreen:
                    songlist = SongController.Create(new DatabaseContext()).GetList()
                        .Where(s => s.Artist == ArtistController.Create(new DatabaseContext()).GetList()
                            .FirstOrDefault(a => a.UserID == UserController.CurrentUser.ID)?.ArtistName).ToList();
                    break;
                default:
                    songlist = new List<Song>();
                    break;
            }

            SongInfoList = SongInfo.ConvertSongListToSongInfo(songlist);
        }

        /**

        public List<Song> SongList = SetScreenForLambda(ScreenName =>
        {
            switch (ScreenName)
            {
                case ScreenNames.PlaylistScreen:
                    return PlaylistSongController.Create(new DatabaseContext())
                        .GetSongsFromPlaylist(MainWindow.CurrentPlayList.ID);
                case ScreenNames.SearchScreen:
                    return
            }
        }); 
        

        public static List<Song> SetScreenForLambda(Func<ScreenNames, List<Song>> callBack)
        {
            return callBack()
        } */


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