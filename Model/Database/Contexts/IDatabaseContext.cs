using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Database.Contexts
{
    public abstract class IDatabaseContext : DbContext
    {
        /**
         * If more tables are added to the database, add them here.
         * Both implementations of this class, require the tables.
         *
         * So errors will occur, if that has been forgotten
         */
        public abstract DbSet<Song> Songs { get; set;  }
        public abstract DbSet<Playlist> Playlists { get; set;  }
        public abstract DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public abstract DbSet<User> Users { get; set; }
        public abstract DbSet<Role> Roles { get; set; }
        public abstract DbSet<Permission> Permissions { get; set; }
        public abstract DbSet<RolePermissions> RolePermissions { get; set; }
        public abstract DbSet<Request> Requests { get; set; }
    }
}
