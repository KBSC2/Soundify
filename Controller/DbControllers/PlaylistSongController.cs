using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistSongController
    {
        private DatabaseContext _context;
        private PlaylistController _playlistController;
        private SongController _songController;

        public PlaylistSongController(DatabaseContext context, DbSet<Playlist> playlist, DbSet<Song> song)
        {
            this._context = context;
            this._playlistController = new PlaylistController(context, playlist);
            this._songController = new SongController(context, song);
        }

        public void addSongToPlaylist(int songID, int playlistID)
        {
            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistID, SongID = songID
            };
            _context.PlaylistSongs.Add(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void deleteSongFromPlaylist(int songID, int playlistID)
        {
            if (!RowExists(songID, playlistID))
                throw new ArgumentOutOfRangeException();

            var playlistSong =
                _context.PlaylistSongs
                    .Where(p => p.PlaylistID == playlistID)
                    .First(s => s.SongID == songID);

            _context.PlaylistSongs.Remove(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        private bool RowExists(int songID, int playlistID)
        {
            return _context.PlaylistSongs
                .Where(p => p.PlaylistID == playlistID)
                .Any(s => s.SongID == songID);
        }
    }
}