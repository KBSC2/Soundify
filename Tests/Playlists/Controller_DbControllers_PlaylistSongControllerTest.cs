using System;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

/*It's important to notice that the database should stay unchanged after running the tests.
 At the start of the test, the database should be in the same state as at the end of the test. */
/*It's important the entries to the database are 0. So at the start and the end the database should
remain untouched*/
namespace Tests.Playlists
{
    [TestFixture]
    public class Controller_DbControllers_PlaylistSongControllerTest
    {
        private MockDatabaseContext context;
        private SongController songController;
        private PlaylistController playlistController;
        private PlaylistSongController playlistSongController;
        private Song song;
        private Playlist playlist;
        [SetUp]
        public void SetUp()
        {
            song = new Song() { Duration = 60, Artist = "Rick Astley", Name = "Never gonna give you up", Path = "../Dit/is/een/path" };
            playlist = new Playlist() {Name = "TESTPLAYLIST", CreationDate = DateTime.Now};
            context = new MockDatabaseContext();
            
            songController = new SongController(context);
            playlistController = new PlaylistController(context);
            playlistSongController = new PlaylistSongController(context);

            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
        }
        [Test]
        public void Controller_PlaylistSongController_AddToPlayList()
        {
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);

            var lastSongID = song.ID;
            var playlistID = playlist.ID;

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
        public void Controller_PlaylistSongController_DeleteFromPlayList()
        {
            //Same Concept as in AddToPlaylist, but it's more explicit for deleting from the playlist.
            //Just use a songID that exists.
            var songID = song.ID;
            var playlistID = playlist.ID;

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
            songController.DeleteItem(song.ID);
            playlistController.DeleteItem(playlist.ID);
        }
    }
}
