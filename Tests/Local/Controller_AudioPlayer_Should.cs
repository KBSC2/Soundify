using NUnit.Framework;
using Controller;
using Controller.DbControllers;
using Model.DbModels;
using Model.Database.Contexts;
using System;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {

        private MockDatabaseContext Context { get; set; }
        private Song Song { get; set; }
        private Song Song2 { get; set; }
        private Playlist Playlist { get; set; }
        private SongController SongController { get; set; }
        private PlaylistSongController PlaylistSongController { get; set; }
        private PlaylistController PlaylistController { get; set; }

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();

            AudioPlayer.Initialize();
            Context = new MockDatabaseContext();
            SongController = new SongController(Context);
            PlaylistSongController = new PlaylistSongController(Context);
            PlaylistController = new PlaylistController(Context);

            Playlist = new Playlist() { Name = "Test", ActivePlaylist = true, CreationDate = DateTime.Now };
            PlaylistController.CreateItem(Playlist);

            Song = new Song() { ID = 1, Artist = "test", Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            Song2 = new Song() { ID = 2, Artist = "test2", Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };

            SongController.CreateItem(Song);
            SongController.CreateItem(Song2);
        }

        [Test, Category("Local")]
        public void AudioPlayer_AddSong_Queue_Contains()
        {
            AudioPlayer.AddSong(Song);
            Assert.IsTrue(AudioPlayer.SongQueue.Contains(Song));
        }

        [Test, Category("Local")]
        public void AudioPlayer_PlayPlaylist_SongQueue_ContainsSongs()
        {
            AudioPlayer.SongQueue.Clear();

            PlaylistSongController.AddSongToPlaylist(Song.ID, Playlist.ID);
            PlaylistSongController.AddSongToPlaylist(Song2.ID, Playlist.ID);

            var playlistsongs = new PlaylistSongController(Context).GetSongsFromPlaylist(Playlist.ID);
            AudioPlayer.PlayPlaylist(playlistsongs);

            bool areEqual = true;
            for (int i = 0; i < playlistsongs.Count; i++)
            {
                if (playlistsongs[i].Song.ID != AudioPlayer.SongQueue[i].ID) areEqual = false;
            }
            Assert.IsTrue(areEqual);
            PlaylistSongController.RemoveFromPlaylist(Song.ID, Playlist.ID);
            PlaylistSongController.RemoveFromPlaylist(Song2.ID, Playlist.ID);
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            PlaylistController.DeleteItem(Playlist.ID);
            SongController.DeleteItem(Song.ID);
            SongController.DeleteItem(Song2.ID);
        }
    }
}
