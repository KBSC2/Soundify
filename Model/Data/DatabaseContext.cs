﻿using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Model.Data
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists{ get; set; }
        public DbSet<PlaylistSong> PlaylistSongs{ get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=127.0.0.1;Initial Catalog=Soundify;User ID=SA;Password=Sterk_W@chtw00rd2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For each model required in the database, copy the line below and change both .Entity<<Model>> and .ToTable("<tableName>")
            /*modelBuilder.Entity<T : DbModel>().ToTable("<TableName>");*/
            modelBuilder.Entity<PlaylistSong>().HasKey(ps => new {ps.PlaylistID, ps.SongID});

            modelBuilder.Entity<PlaylistSong>()
                .HasOne<Playlist>(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistID);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne<Song>(ps => ps.Song)
                .WithMany(s => s.PlaylistSongs)
                .HasForeignKey(ps => ps.SongID);


        }
    }
}
