using System;
using System.Collections.Generic;
using System.Text;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

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
    }
}
