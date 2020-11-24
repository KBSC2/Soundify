using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Data
{
    public class DatabaseContext : DbContext
    {
        // @Deprecated, will be removed
        public static bool TEST_DB { get; set; } = false;

        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists{ get; set; }
        public DbSet<PlaylistSong> PlaylistSongs{ get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MSSQL"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlaylistSong>().HasKey(ps => new {ps.PlaylistID, ps.SongID});

            modelBuilder.Entity<Playlist>()
                .Property(p => p.ActivePlaylist)
                .HasDefaultValue(1);

            base.OnModelCreating(modelBuilder);

        }
    }
}
