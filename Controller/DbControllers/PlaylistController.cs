using System;
using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
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

        public virtual void DeactivatePlaylist(int playlistId)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = GetItem(playlistId);
            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        public virtual void DeletePlaylistOnDateStamp()
        {
            GetList().Where(p => p.ActivePlaylist == false && p.DeleteDateTime < DateTime.Now).
                ToList().ForEach(p => DeleteItem(p.ID));
        }

        public virtual List<Playlist> SearchPlayListOnString(List<string> searchTerms, int userId)
        {
            return GetActivePlaylists(userId)
                .Where(playlist => searchTerms.Any(s => playlist.Name != null && playlist.Name.Contains(s)) ||
                                   searchTerms.Any(
                                       s => playlist.Description != null &&
                                            playlist.Description.ToLower().Contains(s.ToLower())) ||
                                   searchTerms.Any(s =>
                                       playlist.Genre != null && playlist.Genre.ToLower().Contains(s.ToLower())))
                .Take(8)
                .ToList();
        }

        public virtual List<Playlist> GetList(int userId)
        {
            return base.GetFilteredList(x => x.UserID == userId);
        }

        public virtual List<Playlist> GetActivePlaylists(int userId)
        {
            return GetList(userId).Where(x => x.ActivePlaylist).ToList();
        }
    }
}