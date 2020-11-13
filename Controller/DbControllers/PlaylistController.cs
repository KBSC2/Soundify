using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistController: DbController<Playlist>
    {
        private DatabaseContext _context;
        private PlaylistController _playlistController;
        public PlaylistController(DatabaseContext context, DbSet<Playlist> playlist) : base(context, playlist)
        {
            this._context = context;
            this._playlistController = new PlaylistController(context, playlist);
        }
        public void AddPlaylist(int playlistID)
        {
            var playlist = new Playlist()
            {
                PlaylistID = playlistID

            };
            _context.Playlists.Add(playlist);
            _context.Entry(playlist).State = EntityState.Added;
            _context.SaveChanges();


        }
    }
}
