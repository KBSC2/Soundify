﻿using System;
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
            playlistSongController = new PlaylistSongController(context, context.Playlists, context.Songs);
            songController = new SongController(context, context.Songs);
            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
        }
        [Test]
        public void AddToPlayList()
        {
            
            playlistSongController.addSongToPlaylist(song.ID, playlist.ID);

            var lastSongID = songController.GetLastItem().ID;
            var playlistID = playlistController.GetLastItem().ID;

            var existsInPlaylist = playlistSongController.RowExists(lastSongID, playlistID);
            Assert.IsTrue(existsInPlaylist);


        }
        [Test]
        public void DeleteFromPlayList()
        {
            //Just use songID one on the latest playlist.
            var songID = songController.GetLastItem().ID;
            var playlistID = playlistController.GetLastItem().ID;

            var existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);

            playlistSongController.addSongToPlaylist(songID, playlistID);

            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsTrue(existsInPlaylist);

            playlistSongController.removeSongFromPlaylist(songID, playlistID);

            existsInPlaylist = playlistSongController.RowExists(songID, playlistID);
            Assert.IsFalse(existsInPlaylist);
        }

        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(songController.GetLastItem().ID);
            playlistController.DeleteItem(playlistController.GetLastItem().ID);
        }
    }
}
