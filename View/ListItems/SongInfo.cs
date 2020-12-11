using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public static List<SongInfo> ConvertSongListToSongInfo(Playlist playlist, List<PlaylistSong> songs)
        {
            var playlistSongController = PlaylistSongController.Create(new DatabaseContext());

            return songs.Select(song => new SongInfo(song.Song, playlistSongController.GetPlaylistSong(playlist.ID, song.SongID)))
                .ToList();
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