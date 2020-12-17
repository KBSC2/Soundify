using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_QueueController_Should
    {
        private MockDatabaseContext context;
        private Artist artist;
        private Song song;
        private Song song2;
        private SongController songController;
        private List<Song> queue;
        private ArtistController artistController;

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();

            context = new MockDatabaseContext();
            AudioPlayer.Create(context);
            songController = SongController.Create(context);
            artistController = ArtistController.Create(context);

            artist = new Artist();
            artistController.CreateItem(artist);

            song = new Song() { ID = 1, Artist = artist.ID, Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            song2 = new Song() { ID = 2, Artist = artist.ID, Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };

            songController.CreateItem(song);
            songController.CreateItem(song2);

            AudioPlayer.Instance.ClearQueue();
            AudioPlayer.Instance.AddSongToSongQueue(song);
            AudioPlayer.Instance.AddSongToSongQueue(song2);

            queue = AudioPlayer.Instance.Queue;
        }

        [Test, Category("Local")]
        public void QueueController_Queue_RemoveSong()
        {
            QueueController.Instance.DeleteSongFromQueue(song2);

            CollectionAssert.DoesNotContain(queue, song2);
            CollectionAssert.DoesNotContain(AudioPlayer.Instance.NextInQueue, song2);
        }

        [Test, Category("Local")]
        public void QueueController_Queue_MoveSongUpDown()
        {
            int indexSong1 = queue.IndexOf(song);
            int indexSong2 = queue.IndexOf(song2);

            QueueController.Instance.SwapSongs(indexSong1, indexSong2, queue);

            Assert.AreEqual(queue.IndexOf(song), indexSong2);
            Assert.AreEqual(queue.IndexOf(song2), indexSong1);
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
            artistController.DeleteItem(artist.ID);
        }
    }
}
