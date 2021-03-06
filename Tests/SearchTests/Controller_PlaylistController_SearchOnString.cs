﻿using System.Collections.Generic;
using System.Linq;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.SearchTests
{
    [TestFixture]
    public class Controller_Playlist_SearchOnString
    {
        private PlaylistController playlistController;
        private User user;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            var userController = UserController.Create(mock);
            user = userController.GetItem(1);

            playlistController = PlaylistController.Create(mock);
            playlistController.CreateItem(new Playlist()
                { ID = 1, Name = "PlaylistNameTest", UserID = 1, ActivePlaylist = true });
            playlistController.CreateItem(new Playlist()
                { ID = 2, Name = "PlaylistDescriptionTest", Description = "I really wanna test this", UserID = 1, ActivePlaylist = true });
            playlistController.CreateItem(new Playlist()
                { ID = 3, Name = "PlaylistGenreTest", Genre = "Metal", UserID = 1, ActivePlaylist = true });

            user.Playlists = new List<Playlist>()
                { playlistController.GetItem(1), playlistController.GetItem(2), playlistController.GetItem(3) };
        }

        [TestCase("PlaylistNameTest", 1)]           // playlistName
        [TestCase("stNa", 1)]                       // playlistName
        [TestCase("I really wanna test this", 2)]   // playlistDescription
        [TestCase("wanna test", 2)]                 // playlistDescription
        [TestCase("Metal", 3)]                      // playlistGenre
        [TestCase("etal", 3)]                       // playlistGenre
        public void PlaylistController_Search(string search, int expectedId)
        {
            Assert.IsTrue(playlistController.SearchPlayListOnString(new List<string>() { search })
                .Any(x => x.ID == expectedId));
        }

        [TestCase(new [] { "PlaylistNameTest", "I really wanna test this", "Metal" }, new [] {1, 2, 3} )]
        [TestCase(new [] { "meTest", "test this", "eta" }, new [] {1, 2, 3} )]
        public void PlaylistController_Multiple(string[] searches, int[] expected)
        {
            Assert.IsFalse(playlistController.SearchPlayListOnString(searches.ToList())
                .Any(x => !expected.Contains(x.ID)));
        }


        [TearDown]
        public void TearDown()
        {
            foreach (var i in new [] {1, 2, 3})
                playlistController.DeleteItem(i);
        }
    }
}