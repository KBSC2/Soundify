using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistSongController : DbController<PlaylistSong>
    {
        public PlaylistSongController(DatabaseContext context, DbSet<PlaylistSong> set) : base(context, set)
        {

        }
    }
}
