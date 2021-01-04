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
            this.set = context.PlaylistSongs;
        }

        /**
         * Get all PlaylistSongs from the database
         *
         * @return List<PlaylistSong> All playlist songs in the table
         */
        public List<PlaylistSong> GetList()
        {
            return set.ToList();
        }

        /**
         * Adds a song to the designated playlist.
         * If the song is already added then immediately returns
         *
         * @param playlist The Playlist to add the song to
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
                Index = playlist.PlaylistSongs?.Count ?? 0, 
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
         * @param Playlist The Playlist to reorder
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
         * @param playlist The playlist to remove the song from
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
         * @param playlist The playlist to check
         * @param songId The id of the song
         *
         * @return bool : Returns if the row exists or not
         */
        public bool RowExists(Playlist playlist, int songId)
        {
            return playlist.PlaylistSongs?.Any(s => s.SongID == songId) ?? false;
        }

        /**
         * gets a specific song from a given playlist
         *
         * @param playlist The playlist to get the song from
         * @param songId the id of the song
         *
         * @return playlistSong : a single song from a the designated playlist
         */
        public PlaylistSong GetPlaylistSong(Playlist playlist, int songId)
        {
            return playlist.PlaylistSongs?.First(x => x.SongID == songId);
        }

        /**
         * Gets the song from a specific index
         *
         * @param playlist The playlist to get the song from
         * @param songId The id of the song
         *
         * @return A single song based on index from a the designated playlist
         */
        public PlaylistSong GetPlaylistSongFromIndex(Playlist playlist, int index)
        {
            return playlist.PlaylistSongs?.First(x => x.Index == index);
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

            if (IsDetached(playlistSong))
                set.Attach(playlistSong);

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

        /**
         * Swap two songs in the playlist
         *
         * @param playlist The playlist to swap songs in
         * @param indexOne Index of the first song
         * @param indexTwo Index of the second song
         */
        public void SwapSongs(Playlist playlist, int indexOne, int indexTwo)
        {
            var songOne = GetPlaylistSongFromIndex(playlist, indexOne);
            var songTwo = GetPlaylistSongFromIndex(playlist, indexTwo);

            songOne.Index = indexTwo;
            songTwo.Index = indexOne;

            UpdatePlaylistSong(songOne);
            UpdatePlaylistSong(songTwo);
        }

        private bool IsDetached(PlaylistSong entity)
        {
            var localEntity = set.Local?.FirstOrDefault(x => Equals(x.PlaylistID, entity.PlaylistID) && Equals(x.SongID, entity.SongID));
            if (localEntity != null) // entity stored in local
                return false;

            return context.Entry(entity).State == EntityState.Detached;
        }
    }
}