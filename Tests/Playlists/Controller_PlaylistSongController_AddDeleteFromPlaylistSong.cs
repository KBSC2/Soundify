using System;
using System.Collections.Generic;
using System.Linq;
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
            song = new Song() { ID = 1, Duration = 60, ArtistID = 1, Name = "Never gonna give you up", Path = "../Dit/is/een/path" };
            song2 = new Song() { ID = 2, Duration = 60, ArtistID = 2, Name = "Gangnam Style", Path = "../Dit/is/een/path", WrittenBy = "Park Jae-Sang, Yoo Gun Hyung", ProducedBy = "Park Jae-Sang, Yoo Gun Hyung", Description = "PSY - ‘I LUV IT’ M/V @ https://youtu.be/Xvjnoagk6GU PSY - ‘New Face’ M / V @https://youtu.be/OwJPPaEyqhI PSY - 8TH ALBUM '4X2=8' on iTunes @ https://smarturl.it/PSY_8thAlbum PSY - GANGNAM STYLE(강남스타일) on iTunes @ http://smarturl.it/PsyGangnam #PSY #싸이 #GANGNAMSTYLE #강남스타일"};
            playlist = new Playlist() { ID = 10, Name = "TESTPLAYLIST", CreationDate = DateTime.Now, PlaylistSongs = new List<PlaylistSong>()};
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
            playlistSongController.AddSongToPlaylist(playlist, song.ID);
            UpdatePlaylistSongs(playlist);
            Assert.IsTrue(playlistSongController.RowExists(playlist, song.ID));
        }

        [Test]
        public void PlaylistSongController_DeleteFromPlayList()
        {
            playlistSongController.AddSongToPlaylist(playlist, song.ID);
            UpdatePlaylistSongs(playlist);
            playlistSongController.RemoveFromPlaylist(playlist, song.ID);
            UpdatePlaylistSongs(playlist);

            Assert.IsFalse(playlistSongController.RowExists(playlist, song.ID));
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
            playlistSongController.AddSongToPlaylist(playlist, song.ID);
            playlistSongController.AddSongToPlaylist(playlist, song2.ID);
            UpdatePlaylistSongs(playlist);

            int indexSong1 = playlistSongController.GetPlaylistSong(playlist, song.ID).Index;
            int indexSong2 = playlistSongController.GetPlaylistSong(playlist, song2.ID).Index;

            playlistSongController.SwapSongs(playlist, indexSong1, indexSong2);
            UpdatePlaylistSongs(playlist);

            Assert.AreEqual(playlistSongController.GetPlaylistSong(playlist, song.ID).Index, indexSong2);
            Assert.AreEqual(playlistSongController.GetPlaylistSong(playlist, song2.ID).Index, indexSong1);
        }

        //Everytime you test, remove the added items out of the database.
        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
            playlistController.DeleteItem(playlist.ID);
        }

        // Use this to recreate the foriegn key on playlist.PlaylistSongs
        private void UpdatePlaylistSongs(Playlist playlist)
        {
            playlist.PlaylistSongs = playlistSongController.GetList().Where(s => s.PlaylistID == playlist.ID).ToList();
        }
    }
}
