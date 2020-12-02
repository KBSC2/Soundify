using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class ArtistController : DbController<Artist>
    {
        public ArtistController(IDatabaseContext context) : base(context, context.Artists)
        {
        }

        public int? GetArtistIDFromUserID(int userID)
        {
            return new ArtistController(new DatabaseContext()).GetList()
                .FirstOrDefault(a => a.UserID == userID)?.ID;
        }
    }
}
