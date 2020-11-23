using NUnit.Framework;
using Controller;
using Model;
using Controller.DbControllers;
using Model.DbModels;
using Model.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Tests
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {

        private DatabaseContext Context { get; set; }
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
            Context = new DatabaseContext();
            SongController = new SongController(Context);
            PlaylistSongController = new PlaylistSongController(Context);
            PlaylistController = new PlaylistController(Context);

            Playlist = new Playlist() { Name = "Test", ActivePlaylist = true, CreationDate = DateTime.Now };
            PlaylistController.CreateItem(Playlist);

            Song = new Song() { Artist = "test", Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            Song2 = new Song() { Artist = "test2", Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };

            SongController.CreateItem(Song);
            SongController.CreateItem(Song2);
        }

        [Test, Category("Local")]
        public void AddSong_Queue_Contains()
        {
            AudioPlayer.AddSong(Song);
            Assert.IsTrue(AudioPlayer.SongQueue.Contains(Song));
        }

        [Test, Category("Local")]
        public void PlayPlaylist_SongQueue_ContainsSongs()
        {
            AudioPlayer.SongQueue.Clear();

            PlaylistSongController.AddSongToPlaylist(Song.ID, Playlist.ID);
            PlaylistSongController.AddSongToPlaylist(Song2.ID, Playlist.ID);

            AudioPlayer.PlayPlaylist(Playlist);
            var playlistsongs = new PlaylistSongController(Context).GetSongsFromPlaylist(Playlist.ID);

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
