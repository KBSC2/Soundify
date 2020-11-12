using System;
using System.Collections.Generic;
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
    }
}