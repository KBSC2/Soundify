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
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns PlaylistController : the proxy with a instance of this controller included
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
         *
         *  @param Item The playlist data object
         *
         *  @return void
         */
        [HasPermission(Permission = Permissions.PlaylistCreate, MaxValue = Permissions.PlaylistLimit)]
        public override void CreateItem(Playlist item)
        {
            base.CreateItem(item);
        }

        /**
         * A playlist is set to de-activated upon calling this function
         * @param playlistId the id of designated playlist
         *
         * @return void
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
         *
         *  @return void
         */
        public virtual void DeletePlaylistOnDateStamp()
        {
            GetList().Where(p => p.ActivePlaylist == false && p.DeleteDateTime < DateTime.Now).
                ToList().ForEach(p => DeleteItem(p.ID));
        }

        /**
         * The playlist gets selected on Name, Description, Genre
         *
         * @param searchTerms a list of strings containing searchterms
         *
         * @returns List<Playlist> : a list of maximum 8 playlist based on the searchTerms
         */
        public virtual List<Playlist> SearchPlayListOnString(List<string> searchTerms, int userId)
        {
            return GetActivePlaylists()
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

        /**
         * Gets a list of active playlists
         *
         * @param userId the of the currentUser
         *
         * @return A list of active playlist
         */
        public virtual List<Playlist> GetActivePlaylists(int userId)
        {
            return GetList(userId).Where(x => x.ActivePlaylist).ToList();
        }

        /**
         * Gets a list of all active playlists
         *
         * @return A list of active playlists
         */
        public virtual List<Playlist> GetActivePlaylists()
        {
            return GetList().Where(x => x.ActivePlaylist).ToList();
        }
    }
}