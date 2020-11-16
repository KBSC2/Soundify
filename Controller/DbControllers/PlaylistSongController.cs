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

        public void AddSongToPlaylist(int songID, int playlistID)
        {
            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistID, SongID = songID
            };
            _context.PlaylistSongs.Add(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Added;
            _context.SaveChanges();

            
        }

        public List<Song> GetSongsFromPlaylist(int playlistID)
        {
            List<PlaylistSong> playlistSongs = _context.PlaylistSongs.Where(ps => ps.PlaylistID == playlistID).ToList();

            List<Song> songs = new List<Song>();
            foreach (var playlistSong in playlistSongs)
            {
                songs.Add(_songController.GetItem(playlistSong.SongID));
            }

            return songs;
        }
    }
}