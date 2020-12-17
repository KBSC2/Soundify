using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests.Playlists
{
    [TestFixture]
    public class Controller_PlaylistController_GetActivePlaylistsShould
    {
        private PlaylistController playlistController;
        private Playlist testPlaylist1;
        private Playlist testPlaylist2;
        private User user;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            playlistController = PlaylistController.Create(mock);
            user = UserController.Create(mock).GetItem(1);
            user.Playlists = new List<Playlist>();
            testPlaylist1 = new Playlist()
                { ID = 1, Name = "TestPlaylist1", ActivePlaylist = true,  CreationDate = DateTime.Now, UserID = 1, User = user };
            testPlaylist2 = new Playlist()
                { ID = 2, Name = "TestPlaylist2", ActivePlaylist = false, CreationDate = DateTime.Now, UserID = 1, User = user };
        }

        [Test]
        public void PlaylistController_GetActivePlaylists()
        {
            user.Playlists.Add(playlistController.CreateItem(testPlaylist1));
            user.Playlists.Add(playlistController.CreateItem(testPlaylist2));

            testPlaylist2.ActivePlaylist = false;
            playlistController.UpdateItem(testPlaylist2);

            var result = playlistController.GetActivePlaylists(user);
            Assert.True(result.Contains(testPlaylist1));
            Assert.False(result.Contains(testPlaylist2));
        }

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            playlistController.DeleteItem(testPlaylist1.ID);
            playlistController.DeleteItem(testPlaylist2.ID);
        }
    }
}