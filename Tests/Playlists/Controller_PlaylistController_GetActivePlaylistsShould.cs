using System;
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
        private MockDatabaseContext _context;
        private PlaylistController _playlistController;
        private Playlist _testPlaylist1;
        private Playlist _testPlaylist2;


        [SetUp]
        public void SetUp()
        {
            _context = new MockDatabaseContext();
            _playlistController = new PlaylistController(_context);
            _testPlaylist1 = new Playlist()
                {Name = "TestPlaylist1", ActivePlaylist = true, CreationDate = DateTime.Now, UserID = 1};
            _testPlaylist2 = new Playlist()
                { Name = "TestPlaylist2", ActivePlaylist = false, CreationDate = DateTime.Now, UserID = 1};
        }

        [Test]
        public void PlaylistController_GetActivePlaylists()
        {
            _playlistController.CreateItem(_testPlaylist1);
            _playlistController.CreateItem(_testPlaylist2);

            _testPlaylist2.ActivePlaylist = false;
            _playlistController.UpdateItem(_testPlaylist2);

            var result = _playlistController.GetActivePlaylists(1);
            Assert.True(result.Contains(_testPlaylist1));
            Assert.False(result.Contains(_testPlaylist2));
        }

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            _playlistController.DeleteItem(_testPlaylist1.ID);
            _playlistController.DeleteItem(_testPlaylist2.ID);
        }
    }
}