using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;

namespace View
{
    public class SongInfo
    {
        public Song Song { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Added { get; set; }
        public int Index { get; set; }

        public SongInfo(Song song, PlaylistSong playlistSong)
        {
            Song = song;
            Duration = TimeSpan.FromSeconds(song.Duration);
            Added = playlistSong.Added;
            Index = playlistSong.Index;
        }

        public SongInfo(Song song)
        {
            Song = song;
        }

        public static List<SongInfo> ConvertSongListToSongInfo(Playlist playlist, List<PlaylistSong> songs)
        {
            var playlistSongController = new PlaylistSongController(new DatabaseContext());
            var songController = new SongController(new DatabaseContext());

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