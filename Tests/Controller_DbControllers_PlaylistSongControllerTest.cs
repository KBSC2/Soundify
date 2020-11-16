using System;
using System.Collections.Generic;
using System.Text;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;

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
        private Playlist playlist;
        [SetUp]
        public void SetUp()
        {
            song = new Song() { Duration = 60, Name = "Never gonna give you up", Path = "../Dit/is/een/path" };
            playlist = new Playlist();
            context = new DatabaseContext();
            songController = new SongController(context, context.Songs);
            playlistController = new PlaylistController(context, context.Playlists);
            playlistSongController = new PlaylistSongController(context);
            songController = new SongController(context, context.Songs);
            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
        }
        [Test]
        public void AddToPlayList()
        {
            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
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

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(songController.GetLastItem().ID);
            playlistController.DeleteItem(playlistController.GetLastItem().ID);
        }
    }
}
