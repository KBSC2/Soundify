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
    public class PlaylistController : DbController<Playlist>
    {

        public PlaylistController(DatabaseContext context) : base(context, context.Playlists)
        {
        }

        public void DeactivatePlaylist(int playlistID)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = GetItem(playlistID);
            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        public void DeletePlaylistOnDateStamp()
        {
            var currentTime = DateTime.Now;
            var playlists = Context.Playlists.Where(p => p.ActivePlaylist == false).ToList();
            foreach (var VARIABLE in playlists.Where(VARIABLE => VARIABLE.DeleteDateTime < currentTime))
            {
                DeleteItem(VARIABLE.ID);
            }
        }

        public List<Playlist> SearchPlayListOnString(List<string> searchTerms)
        {
            var playlists = Context.Playlists.AsEnumerable();
            List<Playlist> searchPlaylists = playlists
                .Where(playlist => searchTerms.Any(s => playlist.Name != null && playlist.Name.Contains(s)) ||
                                   searchTerms.Any(
                                       s => playlist.Description != null && playlist.Description.Contains(s)) ||
                                   searchTerms.Any(s => playlist.Genre != null && playlist.Genre.Contains(s)))
                .ToList();
            return searchPlaylists;
        }
        public List<Playlist> GetActivePlaylists()
        {
            return GetFilteredList(x => x.ActivePlaylist);
        }
    }
}