using System;
using System.Collections.Generic;
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
    }
}
