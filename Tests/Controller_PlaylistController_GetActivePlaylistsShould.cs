using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Controller;
using Controller.DbControllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;
using View.DataContexts;
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    [TestFixture]
    public class Controller_PlaylistController_GetActivePlaylistsShould
    {
        private DatabaseContext _context;
        private PlaylistController _playlistController;
        private Playlist _testPlaylist1;
        private Playlist _testPlaylist2;


        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
            _context = new DatabaseContext();
            _playlistController = new PlaylistController(_context);
            _testPlaylist1 = new Playlist()
                {Name = "TestPlaylist1", ActivePlaylist = true, CreationDate = DateTime.Now};
            _testPlaylist2 = new Playlist()
                { Name = "TestPlaylist2", ActivePlaylist = false, CreationDate = DateTime.Now };
        }

        [Test]
        public void GetActivePlaylists()
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