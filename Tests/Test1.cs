using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Model.DbModels;
using Controller.DbControllers;
using Model.Data;

namespace Tests
{
    [TestClass]
    class Test1
    {
        private DatabaseContext context;
        private SongController songController;
        private PlaylistController playlistController; 
        private PlaylistSongController playlistSongController;
        private Song song;
        private Playlist playlist;
        [SetUp]
        public void Setup()
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
        public void TestAddPlaylist()
        {
            songController.CreateItem(song);
            playlistController.CreateItem(playlist);
            playlistSongController.addSongToPlaylist(song.ID, playlist.ID);
            /*Not working, waiting on Project 4*/
        }
        
    }
}
