using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using Soundify;

namespace View.DataContexts
{
    public class SongListDataContext : INotifyPropertyChanged
    {
        private static SongListDataContext instance;

        public static SongListDataContext Instance => instance ??= new SongListDataContext();

        public SongInfo SelectedSongInfo { get; set; }

        public ScreenNames ScreenName { get; set; }

        public List<string> SongListSearchTerms { get; set; } = new List<string> {""};

        public bool IsSongListScreen => Instance.ScreenName.Equals(ScreenNames.SongListScreen);
        public bool IsPlaylistScreen => Instance.ScreenName.Equals(ScreenNames.PlaylistScreen);

        public List<SongInfo> SongInfoList { get; set; }

        public List<Playlist> AllPlaylists => PlaylistController.Create(DatabaseContext.Instance)
            .GetActivePlaylists(UserController.CurrentUser);

        public void UpdateSongInfoList()
        {
            var artistController = ArtistController.Create(DatabaseContext.Instance);
            var songController = SongController.Create(DatabaseContext.Instance);
            if (!IsPlaylistScreen)
            {
                SelectedSongInfo = null;
            }

            switch (instance.ScreenName)
            {
                case ScreenNames.PlaylistScreen:
                    SongInfoList = SongInfo.ConvertSongListToSongInfo(MainWindow.CurrentPlayList);
                    break;
                case ScreenNames.SearchScreen:
                    SongInfoList = SongInfo.ConvertSongListToSongInfo(songController.SearchSongsOnString(SearchDataContext.Instance.SearchTerms.ToList()));
                    break;
                case ScreenNames.ArtistScreen:
                    SongInfoList = SongInfo.ConvertSongListToSongInfo(songController.GetList().Where(s => s.Artist.Equals(artistController.GetArtistFromUser(UserController.CurrentUser))).ToList());
                    break;
                case ScreenNames.SongListScreen:
                    if(DataContext.Instance.IsAdmin)
                        SongInfoList = SongInfo.ConvertSongListToSongInfo(songController.SearchSongsOnString(SongListSearchTerms.ToList()));
                    else
                        SongInfoList = SongInfo.ConvertSongListToSongInfo(songController.SearchSongsOnString(SongListSearchTerms.ToList())
                            .Where(s => s.Artist.Equals(artistController.GetArtistFromUser(UserController.CurrentUser))).ToList());
                    break;
                case ScreenNames.AlbumSongListScreen:
                    SongInfoList = SongInfo.ConvertSongListToSongInfo(
                        AlbumSongListDataContext.Instance.Album.Songs.Where(s => s.Status.Equals(SongStatus.Approved)).ToList());
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            UpdateSongInfoList();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}