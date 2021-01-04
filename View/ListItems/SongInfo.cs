using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model.DbModels;
using Model.Enums;
using View.DataContexts;

namespace View
{
    public class SongInfo
    {
        public Song Song { get; set; }
        public Artist Artist { get; set; }
        public bool ShowVisibillity { get; set; }
        public string Duration { get; set; }
        public string Added { get; set; }
        public int Index { get; set; }
        public string Playing => AudioPlayer.Instance.CurrentSong == null ? "White" : Song.ID == AudioPlayer.Instance.CurrentSong.ID ? "#FFF78D0E" : "White";

        //Says is not used, but is used in songlist
        public static bool IsPlaylistScreen => SongListDataContext.Instance.ScreenName.Equals(ScreenNames.PlaylistScreen);
        public static bool IsAlbumSonglistScreen => SongListDataContext.Instance.ScreenName.Equals(ScreenNames.AlbumSongListScreen); 


        public SongInfo(Song song, PlaylistSong playlistSong) : this(song)
        {
            Added = playlistSong.Added.ToString("dd-MM-yyyy");
            Index = playlistSong.Index;
        }

        public SongInfo(Song song)
        {
            Song = song;
            Duration = TimeSpan.FromSeconds(song.Duration).ToString("m':'ss");
            Artist = song.Artist;
            ShowVisibillity = song.Album != null && !SongListDataContext.Instance.ScreenName.Equals(ScreenNames.AlbumSongListScreen);
        }

        public static List<SongInfo> ConvertSongListToSongInfo(Playlist playlist)
        {
            return playlist.PlaylistSongs?.Select(song => new SongInfo(song.Song, song)).OrderBy(s => s.Index).ToList() ?? new List<SongInfo>();
        }

        public static List<SongInfo> ConvertSongListToSongInfo(List<Song> songs)
        {
            return songs.Select(song => new SongInfo(song)).ToList();
        }
    }
}