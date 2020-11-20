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
        private SongController _songController;

        public PlaylistSongController(DatabaseContext context)
        {
            this._context = context;
            _songController = new SongController(context);
        }

        public void AddSongToPlaylist(int songID, int playlistID)
        {
            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistID, SongID = songID, Index = _context.PlaylistSongs.Count()
            };
            _context.PlaylistSongs.Add(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Added;
            _context.SaveChanges();
        }

        public void RemoveFromPlaylist(int songID, int playlistID)
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

        public bool RowExists(int songID, int playlistID)
        {
            return _context.PlaylistSongs
                .Where(p => p.PlaylistID == playlistID)
                .Any(s => s.SongID == songID);
        }

        public List<Song> GetSongsFromPlaylist(int playlistID)
        {
            List<PlaylistSong> playlistSongs = _context.PlaylistSongs.Where(ps => ps.PlaylistID == playlistID).ToList();

            List<Song> songs = new List<Song>();
            foreach (var playlistSong in playlistSongs)
            {
                var id = playlistSong.SongID;
                songs.Add(_songController.GetItem(id));
            }

            return songs;
        }

        public PlaylistSong GetPlaylistSong(int playlistID, int songID)
        {
            return _context.PlaylistSongs
                .Where(p => p.PlaylistID == playlistID)
                .First(s => s.SongID == songID);
        }

        public PlaylistSong GetPlaylistSongFromIndex(int playlistID, int index)
        {
            return _context.PlaylistSongs
                .Where(p => p.PlaylistID == playlistID)
                .First(s => s.Index == index);
        }

        public void UpdatePlaylistSong(PlaylistSong playlistSong)
        {
            _context.PlaylistSongs.Update(playlistSong);
            _context.SaveChanges();
        }
    }
}
