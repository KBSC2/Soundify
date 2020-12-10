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

        private MockDatabaseContext context;
        private Song song;
        private Song song2;
        private Playlist playlist;
        private SongController songController;
        private PlaylistSongController playlistSongController;
        private PlaylistController playlistController;

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();

            context = new MockDatabaseContext();
            AudioPlayer.Create(context);
            songController = SongController.Create(context);
            playlistSongController = PlaylistSongController.Create(context);
            playlistController = PlaylistController.Create(context);

            playlist = new Playlist() { Name = "Test", ActivePlaylist = true, CreationDate = DateTime.Now };
            playlistController.CreateItem(playlist);

            song = new Song() { ID = 1, ArtistID = 1, Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            song2 = new Song() { ID = 2, ArtistID = 1, Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };

            songController.CreateItem(song);
            songController.CreateItem(song2);
        }

        [Test, Category("Local")]
        public void AudioPlayer_AddSong_Queue_Contains()
        {
            AudioPlayer.Instance.AddSongToSongQueue(song);
            Assert.IsTrue(AudioPlayer.Instance.Queue.Contains(song));
        }

        [Test, Category("Local")]
        public void AudioPlayer_PlayPlaylist_SongQueue_ContainsSongs()
        {
            AudioPlayer.Instance.ClearQueue();

            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);
            playlistSongController.AddSongToPlaylist(song2.ID, playlist.ID);

            PlaylistSongController.Create(context).GetSongsFromPlaylist(playlist.ID).ContinueWith(res =>
            {
                var playlistSongs = res.Result;
                AudioPlayer.Instance.PlayPlaylist(playlistSongs);

                bool areEqual = true;
                for (int i = 0; i < playlistSongs.Count; i++)
                {
                    if (playlistSongs[i].Song.ID != AudioPlayer.Instance.Queue[i].ID) areEqual = false;
                }

                Assert.IsTrue(areEqual);
                playlistSongController.RemoveFromPlaylist(song.ID, playlist.ID);
                playlistSongController.RemoveFromPlaylist(song2.ID, playlist.ID);
            });
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            playlistController.DeleteItem(playlist.ID);
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
        }
    }
}
