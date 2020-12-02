using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class ArtistController : DbController<Artist>
    {
        public static ArtistController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<ArtistController>(new object[] { context }, context);
        }

        protected ArtistController(IDatabaseContext context) : base(context, context.Artists)
        {
        }

        public int? GetArtistIDFromUserID(int userID)
        {
            return GetList().FirstOrDefault(a => a.UserID == userID)?.ID;
        }
    }
}
