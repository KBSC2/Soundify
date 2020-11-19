using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class SongController: DbController<Song>
    {
        public SongController(DatabaseContext context) : base(context, context.Songs)
        {
        }
    }
}
