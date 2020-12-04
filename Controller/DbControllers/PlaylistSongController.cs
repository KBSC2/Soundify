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

        public static PlaylistSongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PlaylistSongController>(new object[] { context }, context);
        }

        protected PlaylistSongController(IDatabaseContext context)
        {
            this.context = context;
            set = context.PlaylistSongs;
            songController = SongController.Create(context); 
            playlistController = PlaylistController.Create(context);
        }

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

        public void ReorderSongIndexes(int playlistId)
        {
            set.AsEnumerable().Where(x => x.PlaylistID == playlistId)
                .OrderBy(x => x.Index)
                .Select((p,i) => new {song = p, index = i})
                .Where(p => p.song.Index != p.index).ToList()
                .ForEach(p =>
                {
                    p.song.Index = p.index;
                    this.UpdatePlaylistSong(p.song);
                });
        }

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

        public bool RowExists(int songId, int playlistId)
        {
            return set
                .Where(p => p.PlaylistID == playlistId)
                .Any(s => s.SongID == songId);
        }

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

        public PlaylistSong GetPlaylistSong(int playlistId, int songId)
        {
            return GetSongsFromPlaylist(playlistId)
                .First(s => s.SongID == songId);
        }

        public PlaylistSong GetPlaylistSongFromIndex(int playlistId, int index)
        {
            return GetSongsFromPlaylist(playlistId)
                .First(s => s.Index == index);
        }

        public void UpdatePlaylistSong(PlaylistSong playlistSong)
        {
            set.Update(playlistSong);

            if (!RealDatabase()) return;

            context.Entry(playlistSong).State = EntityState.Modified;
            context.SaveChanges();
        }

        public bool RealDatabase()
        {
            return context is DatabaseContext;
        }
    }
}
