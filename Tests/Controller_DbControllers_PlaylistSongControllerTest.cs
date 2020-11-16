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
        public void DeleteSong()
        {
            songController.CreateItem(song);
            var lastID = songController.GetLastItem().ID;
            /*playlistController.CreateItem(playlist);
            playlistSongController.addSongToPlaylist(song.ID, playlist.ID);*/
        }
    }
}
