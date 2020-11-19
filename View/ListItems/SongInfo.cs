using System;
using System.Collections.Generic;
using System.Text;
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

        public SongInfo(Song song, DateTime added)
        {
            Song = song;
            Duration = TimeSpan.FromSeconds(song.Duration);
            Added = added;
        }

        public static List<SongInfo> ConvertSongListToSongInfo(Playlist playlist, List<Song> songs)
        {
            var returnList = new List<SongInfo>();

            foreach (var song in songs)
            {
                var playlistSongController = new PlaylistSongController(new DatabaseContext());

                returnList.Add(new SongInfo(song, playlistSongController.GetPlaylistSong(playlist.ID, song.ID).Added));
            }

            return returnList;
        }
    }
}
