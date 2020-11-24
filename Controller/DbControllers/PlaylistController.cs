using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Playlist> SearchPlayListOnString(List<string> searchTerms, int userID)
        {
            /*var playlists = Context.Playlists.AsEnumerable();*/
            List<Playlist> searchPlaylists = GetActivePlaylists(userID)
                .Where(playlist => searchTerms.Any(s => playlist.Name != null && playlist.Name.Contains(s)) ||
                                   searchTerms.Any(
                                       s => playlist.Description != null && playlist.Description.ToLower().Contains(s.ToLower())) ||
                                   searchTerms.Any(s => playlist.Genre != null && playlist.Genre.ToLower().Contains(s.ToLower())))
                .Take(8)
                .ToList();
            return searchPlaylists;
        }

        public List<Playlist> GetList(int userID)
        {
            return base.GetFilteredList(x => x.UserID == userID);
        }

        public List<Playlist> GetActivePlaylists(int userID)
        {
            return GetList(userID).Where(x => x.ActivePlaylist).ToList();
        }
    }
}