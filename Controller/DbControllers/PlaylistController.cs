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
        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         * @returns the proxy with a instance of this controller included
         */
        public static PlaylistController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PlaylistController>(new object[] { context }, context);
        }

        protected PlaylistController(IDatabaseContext context) : base(context, context.Playlists)
        {
        }

        /**
         *  This is the same function as in the base class,
         *  but this function is restricted by permissions 
         */
        [HasPermission(Permission = Permissions.PlaylistCreate, MaxValue = Permissions.PlaylistLimit)]
        public override void CreateItem(Playlist item)
        {
            base.CreateItem(item);
        }

        /**
         * A playlist is set to de-activated upon calling this function
         */
        public virtual void DeactivatePlaylist(int playlistId)
        {
            DateTime dateTime = DateTime.Now;
            var playlist = GetItem(playlistId);
            playlist.DeleteDateTime = dateTime.AddDays(1);
            playlist.ActivePlaylist = false;
            UpdateItem(playlist);
        }

        /**
         * Determines if the playlist activation date is in the past,
         * if so the playlist gets deleted from the table
         */
        public virtual void DeletePlaylistOnDateStamp()
        {
            GetList().Where(p => p.ActivePlaylist == false && p.DeleteDateTime < DateTime.Now).
                ToList().ForEach(p => DeleteItem(p.ID));
        }

        /**
         * @param searchTerms 
         * @returns a list of maximum 8 playlist based on the searchTerms
         * The playlist gets selected on Name, Description, Genre
         */
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

        /**
         * @return list based on userId
         */
        public virtual List<Playlist> GetList(int userId)
        {
            return base.GetFilteredList(x => x.UserID == userId);
        }

        /**
         * @return a list of active playlist
         */
        public virtual List<Playlist> GetActivePlaylists(int userId)
        {
            return GetList(userId).Where(x => x.ActivePlaylist).ToList();
        }
    }
}