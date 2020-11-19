using NUnit.Framework;
using Controller;
using Model;

namespace Tests
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {
        [SetUp]
        public void SetUp()
        {
            AudioPlayer.Initialize();
        }

        [Test, Category("Local")]
        public void PlaySong_PlaybackState_Playing()
        {
            AudioPlayer.PlaySong(new SongAudioFile("dansenaandegracht.mp3"));
            Assert.IsTrue(AudioPlayer.WaveOutDevice.PlaybackState == NAudio.Wave.PlaybackState.Playing);
        }

        [Test, Category("Local")]
        public void AddSong_Queue_Contains()
        {
            SongAudioFile songAudioFile = new SongAudioFile("dansenaandegracht.mp3");
            AudioPlayer.AddSong(songAudioFile);
            Assert.IsTrue(AudioPlayer.SongQueue.Contains(songAudioFile));
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            AudioPlayer.WaveOutDevice.Stop();
        }
    }
}
