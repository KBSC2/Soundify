using NUnit.Framework;
using Controller;
using Controller.DbControllers;
using Model.DbModels;
using Model.Database.Contexts;
using System;
using System.Collections.Generic;
using NAudio.Wave;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {

        private MockDatabaseContext context;
        private Song song;
        private Song song2;
        private Song song3;
        private Playlist playlist;
        private SongController songController;
        private PlaylistSongController playlistSongController;
        private PlaylistController playlistController;
        private List<PlaylistSong> playlistSongs;

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

            Artist artist = new Artist();
            ArtistController.Create(context).CreateItem(artist);

            song = new Song() { ID = 1, ArtistID = artist.ID, Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            song2 = new Song() { ID = 2, ArtistID = artist.ID, Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };
            song3 = new Song() { ID = 3, ArtistID = artist.ID, Duration = 11, Name = "test3", Path = "songs/untrago.mp3" };

            songController.CreateItem(song);
            songController.CreateItem(song2);
            songController.CreateItem(song3);

            playlistSongController.AddSongToPlaylist(song.ID, playlist.ID);
            playlistSongController.AddSongToPlaylist(song2.ID, playlist.ID);
            playlistSongController.AddSongToPlaylist(song3.ID, playlist.ID);

            playlistSongs = PlaylistSongController.Create(context).GetSongsFromPlaylist(playlist.ID);

            AudioPlayer.Instance.ClearQueue();
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
            AudioPlayer.Instance.PlayPlaylist(playlist);

            bool areEqual = true;
            for (int i = 0; i < playlistSongs.Count; i++)
                if (playlistSongs[i].Song.ID != AudioPlayer.Instance.Queue[i].ID) areEqual = false;

            Assert.IsTrue(areEqual);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Next_NextPlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);
            AudioPlayer.Instance.Next();

            Assert.AreEqual(playlistSongs[1].SongID, AudioPlayer.Instance.CurrentSong.ID);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Loop_SongQueue_ContainsSongs()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);
            AudioPlayer.Instance.Loop();

            Assert.IsTrue(playlistSongs.Count * 2 == AudioPlayer.Instance.Queue.Count);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Loop_ContinuePlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist, 1);
            AudioPlayer.Instance.Loop();
            AudioPlayer.Instance.Next();

            Assert.AreEqual(playlistSongs[0].SongID, AudioPlayer.Instance.CurrentSong.ID);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Previous_PreviousPlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist, 0);
            AudioPlayer.Instance.Prev();
            
            Assert.AreEqual(playlistSongs[0].SongID, AudioPlayer.Instance.CurrentSong.ID);

        }

        [Test, Category("Local")]
        public void AudioPlayer_PlayPause_PausesSong()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);

            AudioPlayer.Instance.PlayPause();
            Task.Delay(100).ContinueWith(x => {
                Assert.AreEqual(AudioPlayer.Instance.WaveOutDevice.PlaybackState, PlaybackState.Paused);

                AudioPlayer.Instance.PlayPause();

                Task.Delay(100).ContinueWith(x => {
                    Assert.AreEqual(AudioPlayer.Instance.WaveOutDevice.PlaybackState, PlaybackState.Playing);
                });
            });
        }

        [Test, Category("Local")]
        public void AudioPlayer_Shuffle_QueueShuffled()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);
            AudioPlayer.Instance.Shuffle();

            Assert.IsFalse(AudioPlayer.Instance.Queue[1].Equals(playlistSongController.GetSongsFromPlaylist(playlist.ID)[1].Song));
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            playlistSongController.RemoveFromPlaylist(song.ID, playlist.ID);
            playlistSongController.RemoveFromPlaylist(song2.ID, playlist.ID);
            playlistSongController.RemoveFromPlaylist(song3.ID, playlist.ID);
            playlistController.DeleteItem(playlist.ID);
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
            songController.DeleteItem(song3.ID);
        }
    }
}
