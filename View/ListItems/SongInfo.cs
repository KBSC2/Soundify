using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using View.DataContexts;

namespace View
{
    public class SongInfo
    {
        public Song Song { get; set; }
        public Artist Artist { get; set; }
        public string Duration { get; set; }
        public DateTime Added { get; set; }
        public int Index { get; set; }
        public string Playing => AudioPlayer.Instance.CurrentSong == null ? "White" : Song.ID == AudioPlayer.Instance.CurrentSong.ID ? "#FFF78D0E" : "White";

        //Says is not used, but is used in songlist
        public static bool IsPlaylistScreen => SongListDataContext.Instance.ScreenName.Equals(ScreenNames.PlaylistScreen);

        public SongInfo(Song song, PlaylistSong playlistSong) : this(song)
        {
            Added = playlistSong.Added;
            Index = playlistSong.Index;
        }

        public SongInfo(Song song)
        {
            Song = song;
            Duration = TimeSpan.FromSeconds(song.Duration).ToString("m':'ss");
            Artist = ArtistController.Create(DatabaseContext.Instance).GetItem(song.ArtistID);
        }

        public static List<SongInfo> ConvertSongListToSongInfo(Playlist playlist)
        {
            var playlistSongController = PlaylistSongController.Create(DatabaseContext.Instance);

            var songs = playlistSongController.GetSongsFromPlaylist(playlist.ID);
            return songs.Select(song => new SongInfo(song.Song, song)).ToList();
        }

        public static List<SongInfo> ConvertSongListToSongInfo(List<Song> songs)
        {
            var returnList = new List<SongInfo>();

            foreach (var song in songs)
            {
                returnList.Add(new SongInfo(song));
            }

            return returnList;
        }
    }
}