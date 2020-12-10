using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public virtual async void DeactivatePlaylist(int playlistId)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = await GetItem(playlistId);

            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        public virtual async void DeletePlaylistOnDateStamp()
        {
            var list = await GetList();
            list.Where(p => p.ActivePlaylist == false && p.DeleteDateTime < DateTime.Now)
                .ToList().ForEach(p => DeleteItem(p.ID));
        }

        public virtual async Task<List<Playlist>> SearchPlayListOnString(List<string> searchTerms, int userId)
        {
            var list = await GetActivePlaylists();
            return list
                .Where(playlist =>
                    searchTerms.Any(s => playlist.Name != null && playlist.Name.Contains(s)) ||
                    searchTerms.Any(
                        s => playlist.Description != null &&
                             playlist.Description.ToLower().Contains(s.ToLower())) ||
                    searchTerms.Any(s =>
                        playlist.Genre != null && playlist.Genre.ToLower().Contains(s.ToLower())))
                .Take(8)
                .ToList();

        }

        public virtual async Task<List<Playlist>> GetActivePlaylists(int userId)
        {
            var list = await GetActivePlaylists();
            return list.Where(x => x.UserID == userId).ToList();
        }


        public virtual async Task<List<Playlist>> GetActivePlaylists()
        {
            var list = await GetList();
            return list.Where(x => x.ActivePlaylist).ToList();
        }
    }
}