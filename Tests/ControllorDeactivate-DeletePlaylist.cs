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
using Assert = NUnit.Framework.Assert;

namespace Tests
{
    [TestFixture]
    public class ControllorDeactivate_DeletePlaylist
    {
        private DatabaseContext context;
        private PlaylistController playlistController;
        private Playlist playlist;


        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
            context = new DatabaseContext();
            playlistController = new PlaylistController(context, context.Playlists);
            playlist = new Playlist(){Name = "TestDeactivateDelete", ActivePlaylist = true, CreationDate = DateTime.Now};

        }
        [Test]
        public void DeactivatePlaylistTest()
        {
            playlistController.CreateItem(playlist);
            playlistController.DeactivatePlaylist(playlist.ID);
            var testVariable = playlistController.GetItem(playlist.ID);
            Assert.IsFalse(testVariable.ActivePlaylist);
            playlistController.DeleteItem(playlist.ID);
            
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DeletePlaylistTest()
        {
            playlistController.CreateItem(playlist);
            playlistController.DeactivatePlaylist(playlist.ID);
            var testVariable = playlistController.GetItem(playlist.ID);
            testVariable.DeleteDateTime = DateTime.Now.AddMilliseconds(1500);
            playlistController.UpdateItem(testVariable);
            playlistController.DeletePlaylistOnDateStamp();
            Thread.Sleep(3000);
            playlistController.GetItem(playlist.ID);
        }

       
    }
}
