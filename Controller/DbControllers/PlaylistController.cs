using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore.Storage;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class PlaylistController : DbController<Playlist>
    {

        public static PlaylistController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PlaylistController>(new object[] { context }, context);
        }

        protected PlaylistController(IDatabaseContext context) : base(context, context.Playlists)
        {
        }

        [HasPermission(Permission = Permissions.PlaylistCreate, MaxValue = Permissions.PlaylistLimit)]
        public override void CreateItem(Playlist item)
        {
            base.CreateItem(item);
        }

        public virtual void DeactivatePlaylist(int playlistID)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = GetItem(playlistID);
            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        public virtual void DeletePlaylistOnDateStamp()
        {
            var currentTime = DateTime.Now;
            var playlists = GetList().Where(p => p.ActivePlaylist == false).ToList();
            foreach (var VARIABLE in playlists.Where(VARIABLE => VARIABLE.DeleteDateTime < currentTime))
            {
                DeleteItem(VARIABLE.ID);
            }
        }

        public virtual List<Playlist> SearchPlayListOnString(List<string> searchTerms, int userID)
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

        public virtual List<Playlist> GetList(int userID)
        {
            return base.GetFilteredList(x => x.UserID == userID);
        }

        public virtual List<Playlist> GetActivePlaylists(int userID)
        {
            return GetList(userID).Where(x => x.ActivePlaylist).ToList();
        }
    }
}