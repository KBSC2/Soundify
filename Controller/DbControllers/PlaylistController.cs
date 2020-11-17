using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistController: DbController<Playlist>
    {

        public PlaylistController(DatabaseContext context, DbSet<Playlist> playlist) : base(context, playlist)
        {
        }

        public void deactivatePlaylist(int playlistID)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = GetItem(playlistID);
            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        public void deletePlaylistOnDateStamp()
        {
            var currentTime = DateTime.Now;
            var playlists = Context.Playlists.Where(p => p.ActivePlaylist == false).ToList();
            foreach (var VARIABLE in playlists.Where(VARIABLE => VARIABLE.DeleteDateTime < currentTime))
            {
                DeleteItem(VARIABLE.ID);
            }
            
        }


    }
}
