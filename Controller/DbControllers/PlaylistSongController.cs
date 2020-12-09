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
        private SongController songController;
        private PlaylistController playlistController;

        private DbSet<PlaylistSong> set;

        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         * @returns the proxy with a instance of this controller included
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
            songController = SongController.Create(context); 
            playlistController = PlaylistController.Create(context);
        }

        /**
         * adds a song to the designated playlist
         * if the song is already added then immediately returns
         */
        public void AddSongToPlaylist(int songID, int playlistID)
        {
            ReorderSongIndexes(playlistID);

            if (GetSongsFromPlaylist(playlistID).Any(x => x.SongID == songID))
                return;

            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistID, 
                SongID = songID, 
                Index = set.Count(), 
                Song = songController.GetItem(songID), 
                Playlist = playlistController.GetItem(playlistID), 
                Added = DateTime.Now
            };

            set.Add(playlistSong);

            if (!RealDatabase()) return;

            context.Entry(playlistSong).State = EntityState.Added;
            context.SaveChanges();
        }

        /**
         * reorders the songs in the playlist based on the index
         */
        public void ReorderSongIndexes(int playlistId)
        {
            set.AsEnumerable()
                .Where(x => x.PlaylistID == playlistId)
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
         * adds a song to the designated playlist
         * @throws ArgumentOutOfRangeException if the song is does not consist in the playlist
         */
        public void RemoveFromPlaylist(int songId, int playlistId)
        {
            if (!RowExists(songId, playlistId))
                throw new ArgumentOutOfRangeException();

            var playlistSong = GetPlaylistSong(playlistId, songId);

            set.Remove(playlistSong);

            if (RealDatabase())
            {
                context.Entry(playlistSong).State = EntityState.Deleted;
                context.SaveChanges();
            }

            ReorderSongIndexes(playlistId);
        }

        /**
         * Determines if the PlaylistSong exist in the table 
         */
        public bool RowExists(int songId, int playlistId)
        {
            return set
                .Where(p => p.PlaylistID == playlistId)
                .Any(s => s.SongID == songId);
        }


        /**
         * @return a list of songs contained in the designated playlist
         */
        public List<PlaylistSong> GetSongsFromPlaylist(int playlistId)
        {
            ReorderSongIndexes(playlistId);
            var songs = set.Where(ps => ps.PlaylistID == playlistId).OrderBy(ps => ps.Index).ToList();
            songs.ForEach(s =>
            {
                s.Song = songController.GetItem(s.SongID);
                s.Playlist = playlistController.GetItem(s.PlaylistID);
            });

            return songs;
        }

        /**
         * @return a single song from a the designated playlist
         */
        public PlaylistSong GetPlaylistSong(int playlistId, int songId)
        {
            return GetSongsFromPlaylist(playlistId)
                .First(s => s.SongID == songId);
        }

        /**
         * @return a single song based on index from a the designated playlist
         */
        public PlaylistSong GetPlaylistSongFromIndex(int playlistId, int index)
        {
            return GetSongsFromPlaylist(playlistId)
                .First(s => s.Index == index);
        }

        /**
         * updates the PlaylistSong
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
         */
        public bool RealDatabase()
        {
            return context is DatabaseContext;
        }
    }
}
