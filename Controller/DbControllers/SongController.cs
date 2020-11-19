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
        public SongController(DatabaseContext context, DbSet<Song> set) : base(context, set)
        {
        }
    }
}
