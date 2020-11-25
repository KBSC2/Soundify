using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Model.DbModels;
using Moq;

namespace Model.Database.Contexts
{
    public class MockDatabaseContext : IDatabaseContext
    {
        public override DbSet<Song> Songs { get; set; }

        public override DbSet<Playlist> Playlists { get; set; }
        public override DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public override DbSet<User> Users{ get; set; }

        public MockDatabaseContext()
        {
            Songs = GetQueryableMockDbSet(new List<Song>());
            Playlists = GetQueryableMockDbSet(new List<Playlist>());
            PlaylistSongs = GetQueryableMockDbSet(new List<PlaylistSong>());
            Users = GetQueryableMockDbSet(new List<User>
            {
                new User() {ID = 1, Email = "gebruiker1@gmail.com", Password = "$^*^$@", Username = "testaccount"}
            });
        }

        public DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>((s) => sourceList.Remove(s));
            return dbSet.Object;
        }
    }
}
