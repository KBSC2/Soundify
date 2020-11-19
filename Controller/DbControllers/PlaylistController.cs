using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class PlaylistController: DbController<Playlist>
    {

        public PlaylistController(DatabaseContext context) : base(context, context.Playlists)
        {
        }


    }
}
