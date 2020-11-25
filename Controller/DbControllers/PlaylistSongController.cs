using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistSongController
    {
        private IDatabaseContext _context;
        private SongController _songController;
        private PlaylistController _playlistController;

        private DbSet<PlaylistSong> _set;

        public PlaylistSongController(IDatabaseContext context)
        {
            this._context = context;
            _set = _context.PlaylistSongs;
            _songController = new SongController(context);
            _playlistController = new PlaylistController(context);
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
                Index = _context.PlaylistSongs.Count(), 
                Song = _songController.GetItem(songID), 
                Playlist = _playlistController.GetItem(playlistID), 
                Added = DateTime.Now
            };

            _set.Add(playlistSong);

            if (!RealDatabase()) return;

            _context.Entry(playlistSong).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void ReorderSongIndexes(int playlistID)
        {
            var playlistSongs = _context.PlaylistSongs.Where(x => x.PlaylistID == playlistID).OrderBy(x => x.Index)
                .ToList();
            for (var index = 0; index < playlistSongs.Count; index++)
            {
                if (playlistSongs[index].Index != index)
                {
                    var playlistSong = playlistSongs[index];
                    playlistSong.Index = index;
                    this.UpdatePlaylistSong(playlistSong);
                }
            }
        }

        public void RemoveFromPlaylist(int songID, int playlistID)
        {
            if (!RowExists(songID, playlistID))
                throw new ArgumentOutOfRangeException();

            var playlistSong = GetPlaylistSong(playlistID, songID);

            _set.Remove(playlistSong);

            if (RealDatabase())
            {
                _context.Entry(playlistSong).State = EntityState.Deleted;
                _context.SaveChanges();
            }

            ReorderSongIndexes(playlistID);
        }

        public bool RowExists(int songID, int playlistID)
        {
            return _set
                .Where(p => p.PlaylistID == playlistID)
                .Any(s => s.SongID == songID);
        }

        public List<PlaylistSong> GetSongsFromPlaylist(int playlistID)
        {
            ReorderSongIndexes(playlistID);
            var songs = _set.Where(ps => ps.PlaylistID == playlistID).OrderBy(ps => ps.Index).ToList();
            songs.ForEach(s =>
            {
                s.Song = _songController.GetItem(s.SongID);
                s.Playlist = _playlistController.GetItem(s.PlaylistID);
            });

            return songs;
        }

        public PlaylistSong GetPlaylistSong(int playlistID, int songID)
        {
            return GetSongsFromPlaylist(playlistID)
                .First(s => s.SongID == songID);
        }

        public PlaylistSong GetPlaylistSongFromIndex(int playlistID, int index)
        {
            return GetSongsFromPlaylist(playlistID)
                .First(s => s.Index == index);
        }

        public void UpdatePlaylistSong(PlaylistSong playlistSong)
        {
            _set.Update(playlistSong);

            if (!RealDatabase()) return;

            _context.Entry(playlistSong).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool RealDatabase()
        {
            return _context is DatabaseContext;
        }
    }
}
