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
        private PlaylistController _playlistController;

        public PlaylistSongController(DatabaseContext context)
        {
            this._context = context;
            _songController = new SongController(context);
            _playlistController = new PlaylistController(context);
        }

        public void AddSongToPlaylist(int songID, int playlistID)
        {
            ReorderSongIndexes(playlistID);
            var playlistSong = new PlaylistSong()
            {
                PlaylistID = playlistID, SongID = songID, Index = _context.PlaylistSongs.Count(), Song = _songController.GetItem(songID), Playlist = _playlistController.GetItem(playlistID)
            };
            _context.PlaylistSongs.Add(playlistSong);
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

            _context.PlaylistSongs.Remove(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Deleted;
            _context.SaveChanges();

            ReorderSongIndexes(playlistID);
        }

        public bool RowExists(int songID, int playlistID)
        {
            return _context.PlaylistSongs
                .Where(p => p.PlaylistID == playlistID)
                .Any(s => s.SongID == songID);
        }

        public List<PlaylistSong> GetSongsFromPlaylist(int playlistID)
        {
            ReorderSongIndexes(playlistID);
            var songs = _context.PlaylistSongs.Where(ps => ps.PlaylistID == playlistID).OrderBy(ps => ps.Index).ToList();
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
            _context.PlaylistSongs.Update(playlistSong);
            _context.Entry(playlistSong).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}