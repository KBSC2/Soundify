using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class AlbumController : DbController<Album>
    {
        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns ArtistController : the proxy with a instance of this controller included
         */
        public static AlbumController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<AlbumController>(new object[] { context }, context);
        }

        protected AlbumController(IDatabaseContext context) : base(context, context.Albums)
        {
        }

        public List<Album> SearchAlbumListOnString(List<string> searchTerms)
        {
            
            return GetList()
                .Where(album => (searchTerms.Any(s => album.AlbumName != null && album.AlbumName.ToLower().Contains(s.ToLower())) ||
                                searchTerms.Any(s => 
                                    album.AlbumArtistSongs
                                        .Where(aas => aas.Album.Equals(album))
                                        .GroupBy(aas => aas.Artist)
                                        .OrderBy(aas => aas.Count())
                                        .Select(aas => aas.Key)
                                        .First().ArtistName.ToLower()
                                        .Contains(s.ToLower())))
                     && (!album.AlbumArtistSongs.Select(aas => aas.Song).Where(s => s.Status.Equals(SongStatus.Approved)).ToList().IsNullOrEmpty())
                                )
                .ToList();
        }
    }
}
