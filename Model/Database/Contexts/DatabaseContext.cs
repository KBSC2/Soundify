using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Database.Contexts
{
    public class DatabaseContext : IDatabaseContext
    {
        // @Deprecated, will be removed
        public override DbSet<Song> Songs { get; set; }
        public override DbSet<Playlist> Playlists { get; set; }
        public override DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public override DbSet<User> Users { get; set;  }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<Artist> Artists { get; set; }
        public override DbSet<Permission> Permissions { get; set; }
        public override DbSet<RolePermissions> RolePermissions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Fetch database settings from the App.Config
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(@"View.dll");
                var connectionString = configuration.ConnectionStrings.ConnectionStrings["MSSQL"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>().HasKey(ps => new {ps.PlaylistID, ps.SongID});

            modelBuilder.Entity<RolePermissions>().HasKey(rp => new { rp.RoleID, rp.PermissionID});

            modelBuilder.Entity<Playlist>()
                .Property(p => p.ActivePlaylist)
                .HasDefaultValue(1);

            base.OnModelCreating(modelBuilder);
        }
    }
}
