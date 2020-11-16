using System;
using System.Collections.Generic;
using System.Text;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;

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
            playlistSongController = new PlaylistSongController(context, context.Playlists, context.Songs);
            songController = new SongController(context, context.Songs);
        }
        [Test]
        public void AddToPlayList()
        {
            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
            playlistSongController.addSongToPlaylist(song.ID, playlist.ID);

            var lastSongID = songController.GetLastItem().ID;
            var playlistID = playlistController.GetLastItem().ID;
            var existsInPlaylist = playlistSongController.RowExists(lastSongID,playlistID);
            Assert.IsTrue(existsInPlaylist);


        }
        [Test]
        public void DeleteFromPlayList()
        {
            //Just use songID one on the latest playlist.
            var songID = 1;
            var playlistID = playlistController.GetLastItem().ID;

            var existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);

            playlistSongController.addSongToPlaylist(songID, playlistID);

            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsTrue(existsInPlaylist);

            playlistSongController.renameSongFromPlaylist(songID, playlistID);

            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);
        }
    }
}
