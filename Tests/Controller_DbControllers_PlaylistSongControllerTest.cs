﻿using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;
using NUnit.Framework.Internal;

/*It's important to notice that the database should stay unchanged after running the tests.
 At the start of the test, the database should be in the same state as at the end of the test. */
/*It's important the entries to the database are 0. So at the start and the end the database should
remain untouched*/
namespace Tests
{
    [TestFixture]
    public class Controller_DbControllers_PlaylistSongControllerTest
    {
        private DatabaseContext context;
        private SongController songController;
        private PlaylistController playlistController;
        private PlaylistSongController playlistSongController;
        private Song song;
        private Song song2;
        private Playlist playlist;
        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();

            song = new Song() { Duration = 60, Artist = "Rick Astley", Name = "Never gonna give you up", Path = "../Dit/is/een/path" };
            song2 = new Song() { Duration = 60, Artist = "PSY", Name = "Gangnam Style", Path = "../Dit/is/een/path", WrittenBy = "Park Jae-Sang, Yoo Gun Hyung", ProducedBy = "Park Jae-Sang, Yoo Gun Hyung", Description = "PSY - ‘I LUV IT’ M/V @ https://youtu.be/Xvjnoagk6GU PSY - ‘New Face’ M / V @https://youtu.be/OwJPPaEyqhI PSY - 8TH ALBUM '4X2=8' on iTunes @ https://smarturl.it/PSY_8thAlbum PSY - GANGNAM STYLE(강남스타일) on iTunes @ http://smarturl.it/PsyGangnam #PSY #싸이 #GANGNAMSTYLE #강남스타일"};
            playlist = new Playlist() {Name = "TESTPLAYLIST", CreationDate = DateTime.Now};
            context = new DatabaseContext();
            
            songController = new SongController(context);
            playlistController = new PlaylistController(context);
            playlistSongController = new PlaylistSongController(context);

            songController.CreateItem(song);
            songController.CreateItem(song2);
            playlistController.CreateItem(playlist);
        }
        [Test]
        public void AddToPlayList()
        {
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);

            var lastSongID = songController.GetLastItem().ID;
            var playlistID = playlistController.GetLastItem().ID;

            var existsInPlaylist = playlistSongController.RowExists(lastSongID, playlistID);
            Assert.IsTrue(existsInPlaylist);

            //After adding, remove it.
            playlistSongController.RemoveFromPlaylist(song.ID, playlist.ID);

            //Remove the added song to the playlist at the end of the test.
            //Extra check to see whether the playlist is removed at the end.
            existsInPlaylist = playlistSongController.RowExists(lastSongID, playlistID);
            Assert.IsFalse(existsInPlaylist);
        }
        [Test]
        public void DeleteFromPlayList()
        {
            //Same Concept as in AddToPlaylist, but it's more explicit for deleting from the playlist.
            //Just use a songID that exists.
            var songID = songController.GetLastItem().ID;
            var playlistID = playlistController.GetLastItem().ID;

            //Before adding
            var existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);

            playlistSongController.AddSongToPlaylist(songID, playlistID);

            //After adding
            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsTrue(existsInPlaylist);

            playlistSongController.RemoveFromPlaylist(songID, playlistID);

            //After removing
            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);
        }

        [Test]
        public void SongInfoAssert()
        {
            Assert.AreEqual("Park Jae-Sang, Yoo Gun Hyung", song2.WrittenBy);
            Assert.AreEqual("Park Jae-Sang, Yoo Gun Hyung", song2.ProducedBy);
            Assert.AreEqual("PSY - ‘I LUV IT’ M/V @ https://youtu.be/Xvjnoagk6GU PSY - ‘New Face’ M / V @https://youtu.be/OwJPPaEyqhI PSY - 8TH ALBUM '4X2=8' on iTunes @ https://smarturl.it/PSY_8thAlbum PSY - GANGNAM STYLE(강남스타일) on iTunes @ http://smarturl.it/PsyGangnam #PSY #싸이 #GANGNAMSTYLE #강남스타일", song2.Description);
        }

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(songController.GetLastItem().ID);
            playlistController.DeleteItem(playlistController.GetLastItem().ID);
        }
    }
}
