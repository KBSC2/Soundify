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
            playlistDelete = new Playlist() 
                { ID = 2, Name = "TestDelete", ActivePlaylist = true, CreationDate = DateTime.Now};

            playlistController.CreateItem(playlist);
            playlistController.CreateItem(playlistDelete);
        }

        [Test]
        public void PlaylistController_DeactivatePlaylistTest_DeactivatedPlaylist()
        {
            playlistController.DeactivatePlaylist(1);
            playlistController.GetItem(1).ContinueWith(res =>
                Assert.IsFalse(res.Result.ActivePlaylist)
            );

        }

        [Test]
        public void PlaylistController_DeletePlaylistTest_DeletedPlaylist()
        {
            playlistController.DeactivatePlaylist(playlistDelete.ID);

            playlistController.GetItem(playlistDelete.ID).ContinueWith(res =>
            {
                var testVariable = res.Result;
                testVariable.DeleteDateTime = DateTime.Now - TimeSpan.FromMilliseconds(1500);

                playlistController.UpdateItem(testVariable);
                playlistController.DeletePlaylistOnDateStamp();

                Assert.Throws<ArgumentOutOfRangeException>((() => playlistController.GetItem(playlistDelete.ID).Start()));
            });
        }
    }
}