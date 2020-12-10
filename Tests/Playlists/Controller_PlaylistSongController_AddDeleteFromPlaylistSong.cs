using System;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using View;
using View.Screens;

/*It's important to notice that the database should stay unchanged after running the tests.
 At the start of the test, the database should be in the same state as at the end of the test. */
/*It's important the entries to the database are 0. So at the start and the end the database should
remain untouched*/
namespace Tests.Playlists
{
    [TestFixture]
    public class Controller_PlaylistSongController_AddDeleteFromPlaylistSong
    {
        private SongController songController;
        private PlaylistController playlistController;
        private PlaylistSongController playlistSongController;
        private Song song;
        private Song song2;
        private Playlist playlist;

        [SetUp]
        public void SetUp()
        {
            song = new Song() { Duration = 60, Artist = "Rick Astley", Name = "Never gonna give you up", Path = "../Dit/is/een/path" };
            song2 = new Song() { Duration = 60, Artist = "PSY", Name = "Gangnam Style", Path = "../Dit/is/een/path", WrittenBy = "Park Jae-Sang, Yoo Gun Hyung", ProducedBy = "Park Jae-Sang, Yoo Gun Hyung", Description = "PSY - ‘I LUV IT’ M/V @ https://youtu.be/Xvjnoagk6GU PSY - ‘New Face’ M / V @https://youtu.be/OwJPPaEyqhI PSY - 8TH ALBUM '4X2=8' on iTunes @ https://smarturl.it/PSY_8thAlbum PSY - GANGNAM STYLE(강남스타일) on iTunes @ http://smarturl.it/PsyGangnam #PSY #싸이 #GANGNAMSTYLE #강남스타일"};
            playlist = new Playlist() {Name = "TESTPLAYLIST", CreationDate = DateTime.Now};
            var context = new MockDatabaseContext();
            
            songController = SongController.Create(context);
            playlistController = PlaylistController.Create(context);
            playlistSongController = PlaylistSongController.Create(context);

            songController.CreateItem(song);
            songController.CreateItem(song2);
            playlistController.CreateItem(playlist);
        }

        [Test]
        public void PlaylistSongController_AddToPlayList()
        {
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);

            var lastSongId = song.ID;
            var playlistId = playlist.ID;

            var existsInPlaylist = playlistSongController.RowExists(lastSongId, playlistId);
            Assert.IsTrue(existsInPlaylist);

            //After adding, remove it.
            playlistSongController.RemoveFromPlaylist(song.ID, playlist.ID);

            //Remove the added song to the playlist at the end of the test.
            //Extra check to see whether the playlist is removed at the end.
            existsInPlaylist = playlistSongController.RowExists(lastSongId, playlistId);
            Assert.IsFalse(existsInPlaylist);
        }

        [Test]
        public void PlaylistSongController_DeleteFromPlayList()
        {
            //Same Concept as in AddToPlaylist, but it's more explicit for deleting from the playlist.
            //Just use a songID that exists.
            var songId = song.ID;
            var playlistId = playlist.ID;

            //Before adding
            var existsInPlaylist = playlistSongController.RowExists(songId, playlistId);
            Assert.IsFalse(existsInPlaylist);

            playlistSongController.AddSongToPlaylist(songId, playlistId);

            //After adding
            existsInPlaylist = playlistSongController.RowExists(songId, playlistId);
            Assert.IsTrue(existsInPlaylist);

            playlistSongController.RemoveFromPlaylist(songId, playlistId);

            //After removing
            existsInPlaylist = playlistSongController.RowExists(songId, playlistId);
            Assert.IsFalse(existsInPlaylist);
        }

        [Test]
        public void SongInfoAssert()
        {
            Assert.AreEqual("Park Jae-Sang, Yoo Gun Hyung", song2.WrittenBy);
            Assert.AreEqual("Park Jae-Sang, Yoo Gun Hyung", song2.ProducedBy);
            Assert.AreEqual("PSY - ‘I LUV IT’ M/V @ https://youtu.be/Xvjnoagk6GU PSY - ‘New Face’ M / V @https://youtu.be/OwJPPaEyqhI PSY - 8TH ALBUM '4X2=8' on iTunes @ https://smarturl.it/PSY_8thAlbum PSY - GANGNAM STYLE(강남스타일) on iTunes @ http://smarturl.it/PsyGangnam #PSY #싸이 #GANGNAMSTYLE #강남스타일", song2.Description);
        }

        [Test]
        public void PlaylistSongController_MoveSongUpDownInPlaylist()
        {
            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);
            playlistSongController.AddSongToPlaylist(song2.ID, playlist.ID);
            int indexSong1 = playlistSongController.GetPlaylistSong(playlist.ID, song.ID).Index;
            int indexSong2 = playlistSongController.GetPlaylistSong(playlist.ID, song2.ID).Index;

            playlistController.SwapSongs(indexSong1, indexSong2, playlist.ID);

            Assert.AreEqual(playlistSongController.GetPlaylistSong(playlist.ID, song.ID).Index, indexSong2);
            Assert.AreEqual(playlistSongController.GetPlaylistSong(playlist.ID, song2.ID).Index, indexSong1);
        }

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
            playlistController.DeleteItem(playlist.ID);
        }
    }
}
