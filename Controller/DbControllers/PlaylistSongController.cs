using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistSongController
    {
        private IDatabaseContext context;

        private DbSet<PlaylistSong> set;

        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param Context gets the databaseContext
         *
         * @returns The proxy with a instance of this controller included
         */
        public static PlaylistSongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PlaylistSongController>(new object[] { context }, context);
        }

        /**
         * This controller is made for a join table,
         * If the class is constructed, the song controller and the playlist controller are created as well
         */
        protected PlaylistSongController(IDatabaseContext context)
        {
            this.context = context;
            set = context.PlaylistSongs;
        }

        /**
         * Adds a song to the designated playlist.
         * If the song is already added then immediately returns
         *
         * @param playlistId The id of the designated playlist
         * @param songId The id of the song
         *
         * @return void
         */
        public PlaylistSong AddSongToPlaylist(Playlist playlist, int songId)
        {
            ReorderSongIndexes(playlist);

            if (RowExists(playlist, songId))
                return null;

            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlist.ID, 
                SongID = songId, 
                Index = set.Count(), 
                Added = DateTime.Now
            };

            set.Add(playlistSong);

            if (RealDatabase())
            {
                context.Entry(playlistSong).State = EntityState.Added;
                context.SaveChanges();
            }

            return playlistSong;
        }

        /**
         * Reorders the indexes of the playlist from 0 to the amount of songs in a playlist
         *
         * @param PlaylistId The id of the designated playlist
         *
         * @return void
         */
        public void ReorderSongIndexes(Playlist playlist)
        {
            playlist.PlaylistSongs?.AsEnumerable()
                .OrderBy(x => x.Index)
                .Select((p,i) => new {song = p, index = i})
                .Where(p => p.song.Index != p.index).ToList()
                .ForEach(p =>
                {
                    p.song.Index = p.index;
                    this.UpdatePlaylistSong(p.song);
                });
        }

        /**
         * Adds a song to the designated playlist
         *
         * @param playlistId the id of the designated playlist
         * @param songId the id of the song
         *
         * @throws ArgumentOutOfRangeException if the song is does not consist in the playlist
         *
         * @return void
         */
        public void RemoveFromPlaylist(Playlist playlist, int songId)
        {
            if (!RowExists(playlist, songId))
                throw new ArgumentOutOfRangeException();

            var playlistSong = GetPlaylistSong(playlist, songId);

            set.Remove(playlistSong);

            if (RealDatabase())
            {
                context.Entry(playlistSong).State = EntityState.Deleted;
                context.SaveChanges();
            }

            ReorderSongIndexes(playlist);
        }

        /**
         * Determines if the PlaylistSong exist in the table
         *
         * @param playlistId The id of the designated playlist
         * @param songId The id of the song
         *
         * @return bool : Returns if the row exists or not
         */
        public bool RowExists(Playlist playlist, int songId)
        {
            return playlist.PlaylistSongs.Any(s => s.SongID == songId);

            /*return set
                .Where(p => p.PlaylistID == playlistId)
                .Any(s => s.SongID == songId);*/
        }


        /**
         * Gets the songs from the playlist given in the parameter
         *
         * @param playlistId The id of the designated playlist
         *
         * @return A list of songs contained in the designated playlist
         */
        /*public List<PlaylistSong> GetSongsFromPlaylist(int playlistId)
        {
            ReorderSongIndexes(playlistId);
            var songs = set.Where(ps => ps.PlaylistID == playlistId).OrderBy(ps => ps.Index).ToList();
            songs.ForEach(s =>
            {
                s.Song = songController.GetItem(s.SongID);
                s.Playlist = playlistController.GetItem(s.PlaylistID);
            });

            return songs;
        }*/

        /**
         * gets a specific song from a given playlist
         *
         * @param playlistId the id of the designated playlist
         * @param songId the id of the song
         *
         * @return playlistSong : a single song from a the designated playlist
         */
        public PlaylistSong GetPlaylistSong(Playlist playlist, int songId)
        {
            return playlist.PlaylistSongs.First(x => x.SongID == songId);
            /*return GetSongsFromPlaylist(playlistId)
                .First(s => s.SongID == songId);*/
        }

        /**
         * Gets the song from a specific index
         *
         * @param playlistId The id of the designated playlist
         * @param songId The id of the song
         *
         * @return A single song based on index from a the designated playlist
         */
        public PlaylistSong GetPlaylistSongFromIndex(Playlist playlist, int index)
        {
            return playlist.PlaylistSongs.First(x => x.Index == index);
            /*return GetSongsFromPlaylist(playlistId)
                .First(s => s.Index == index);*/
        }

        /**
         * Updates the PlaylistSong
         *
         * @param playlistSong The data object PlaylistSong
         *
         * @return void
         */
        public void UpdatePlaylistSong(PlaylistSong playlistSong)
        {
            set.Update(playlistSong);

            if (!RealDatabase()) return;

            context.Entry(playlistSong).State = EntityState.Modified;
            context.SaveChanges();
        } 
        
        /**
         * Determine if the database is a real database, or a mock database
         *
         * @return bool : Checks if it is a real database or not
         */
        public bool RealDatabase()
        {
            return context is DatabaseContext;
        }

        public void SwapSongs(Playlist playlist, int indexOne, int indexTwo)
        {
            var songOne = GetPlaylistSongFromIndex(playlist, indexOne);
            var songTwo = GetPlaylistSongFromIndex(playlist, indexTwo);

            songOne.Index = indexTwo;
            songTwo.Index = indexOne;

            UpdatePlaylistSong(songOne);
            UpdatePlaylistSong(songTwo);
        }
    }
}
