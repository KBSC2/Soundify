using System;
using System.Threading;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Tests.Playlists
{
    [TestFixture]
    public class Controller_PlaylistControllor_DeactivateDeletePlaylist
    {
        private PlaylistController playlistController;
        private Playlist playlist;
        private Playlist playlistDelete;

        [SetUp]
        public void SetUp()
        {
            playlistController = PlaylistController.Create(new MockDatabaseContext());
            playlist = new Playlist()
                { ID = 1, Name = "TestDeactivateDelete", ActivePlaylist = true, CreationDate = DateTime.Now};
            playlistDelete = new Playlist() {ID = 2, Name = "TestDelete", ActivePlaylist = true, CreationDate = DateTime.Now};
        }

        [Test]
        public void PlaylistController_DeactivatePlaylistTest_DeactivatedPlaylist()
        {
            playlistController.CreateItem(playlist);
            playlistController.DeactivatePlaylist(playlist);
            var testVariable = playlistController.GetItem(playlist.ID);
            Assert.IsFalse(testVariable.ActivePlaylist);
            playlistController.DeleteItem(playlist.ID);
        }

        [Test]
        public void PlaylistController_DeletePlaylistTest_DeletedPlaylist()
        {
            playlistController.CreateItem(playlistDelete);
            playlistController.DeactivatePlaylist(playlistDelete);
            var testVariable = playlistController.GetItem(playlistDelete.ID);
            testVariable.DeleteDateTime = DateTime.Now.AddMilliseconds(1500);
            playlistController.UpdateItem(testVariable);
            Thread.Sleep(2000);
            playlistController.DeletePlaylistOnDateStamp();
            Thread.Sleep(3000);
            Assert.Throws<ArgumentOutOfRangeException>((() => playlistController.GetItem(playlistDelete.ID)));
        }
    }
}