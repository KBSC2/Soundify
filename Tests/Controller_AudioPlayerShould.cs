using NUnit.Framework;
using Controller;
using Model;
using Controller.DbControllers;
using Model.DbModels;
using Model.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {

        private DatabaseContext Context { get; set; }
        private Song Song { get; set; }

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
            AudioPlayer.Initialize();
            Context = new DatabaseContext();
            Song = new SongController(Context).GetItem(9);
        }

        [Test, Category("Local")]
        public void PlaySong_PlaybackState_Playing()
        {
            AudioPlayer.PlaySong(Song);
            Thread.Sleep(500);
            Assert.IsTrue(AudioPlayer.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing);
            Thread.Sleep(500);
            AudioPlayer.WaveOutDevice.Stop();
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
            Playlist playlist = new PlaylistController(Context).GetItem(19);
            AudioPlayer.PlayPlaylist(playlist);
            var playlistsongs = new PlaylistSongController(Context).GetSongsFromPlaylist(playlist.ID);

            bool areEqual = true;
            for (int i = 0; i < playlistsongs.Count; i++)
            {
                if (playlistsongs[i].ID != AudioPlayer.SongQueue[i].ID) areEqual = false;
            }
            Assert.IsTrue(areEqual);
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            AudioPlayer.WaveOutDevice.Stop();
        }
    }
}
