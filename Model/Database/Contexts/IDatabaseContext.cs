using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Database.Contexts
{
    public abstract class IDatabaseContext : DbContext
    {
        public abstract DbSet<Song> Songs { get; set;  }
        public abstract DbSet<Playlist> Playlists { get; set;  }
        public abstract DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public abstract DbSet<User> Users { get; set; }
    }
}
