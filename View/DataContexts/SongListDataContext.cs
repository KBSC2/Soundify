using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
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

        public ScreenNames ScreenName { get; set; }

        public List<string> SongListSearchTerms { get; set; } = new List<string>(){""};

        public bool IsSongListScreen => Instance.ScreenName.Equals(ScreenNames.SongListScreen);

        public bool IsPlaylistScreen => Instance.ScreenName.Equals(ScreenNames.PlaylistScreen);

        public void UpdateSongInfoList()
        {
            var artistController = ArtistController.Create(DatabaseContext.Instance);
            List<Song> songlist = new List<Song>();
            switch (instance.ScreenName)
            {
                case ScreenNames.PlaylistScreen:
                    songlist = PlaylistSongController.Create(DatabaseContext.Instance)
                        .GetSongsFromPlaylist(Soundify.MainWindow.CurrentPlayList.ID).Select(ps => ps.Song).ToList();
                    break;
                case ScreenNames.SearchScreen:
                    songlist = SongController.Create(DatabaseContext.Instance)
                        .SearchSongsOnString(SearchDataContext.Instance.SearchTerms.ToList());
                    break;
                case ScreenNames.ArtistScreen:
                    songlist = SongController.Create(DatabaseContext.Instance).GetList()
                        .Where(s => s.Artist.Equals(
                            artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID))).ToList();
                    break;
                case ScreenNames.SongListScreen:
                    if (DataContext.Instance.IsAdmin != null && (bool)DataContext.Instance.IsAdmin)
                    {
                        songlist = SongController.Create(DatabaseContext.Instance)
                            .SearchSongsOnString(SongListSearchTerms.ToList());
                    }
                    else if (DataContext.Instance.IsArtist != null && (bool) DataContext.Instance.IsArtist)
                    {
                        songlist = SongController.Create(DatabaseContext.Instance)
                            .SearchSongsOnString(SongListSearchTerms.ToList())
                            .Where(s => s.Artist.Equals(
                                artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID))).ToList();
                    }
                    break;
            }

            SongInfoList = IsPlaylistScreen
                ? SongInfo.ConvertSongListToSongInfo(MainWindow.CurrentPlayList)
                : SongInfo.ConvertSongListToSongInfo(songlist);
        }

        public List<SongInfo> SongInfoList { get; set; }

        public List<Playlist> AllPlaylists => PlaylistController.Create(DatabaseContext.Instance)
            .GetActivePlaylists(UserController.CurrentUser.ID);

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            UpdateSongInfoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}