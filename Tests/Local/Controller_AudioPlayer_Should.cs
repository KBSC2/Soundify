using NUnit.Framework;
using Controller;
using Controller.DbControllers;
using Model.DbModels;
using Model.Database.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using System.Threading.Tasks;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_AudioPlayerShould
    {
        
        private ArtistController artistController;
        private SongController songController;
        private PlaylistSongController playlistSongController;
        private PlaylistController playlistController;

        private Playlist playlist;
        private Song song;
        private Song song2;
        private Song song3;

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();

            var context = new MockDatabaseContext();
            AudioPlayer.Create(context);
            songController = SongController.Create(context);
            playlistSongController = PlaylistSongController.Create(context);
            playlistController = PlaylistController.Create(context);
            artistController = ArtistController.Create(context);

            playlist = new Playlist() { ID = 10, Name = "Test", ActivePlaylist = true, CreationDate = DateTime.Now, PlaylistSongs = new List<PlaylistSong>()};
            playlistController.CreateItem(playlist);

            var artist = new Artist() { ID = 1, UserID = 1, User = UserController.Create(context).GetItem(1)};
            ArtistController.Create(context).CreateItem(artist);

            song = new Song()  { ID = 1, ArtistID = artist.ID, Artist = artist, Duration = 11, Name = "test", Path = "songs/dansenaandegracht.mp3" };
            song2 = new Song() { ID = 2, ArtistID = artist.ID, Artist = artist, Duration = 11, Name = "test2", Path = "songs/untrago.mp3" };
            song3 = new Song() { ID = 3, ArtistID = artist.ID, Artist = artist, Duration = 11, Name = "test3", Path = "songs/untrago.mp3" };

            songController.CreateItem(song);
            songController.CreateItem(song2);
            songController.CreateItem(song3);

            playlist.PlaylistSongs.Add(playlistSongController.AddSongToPlaylist(playlist, song.ID ));
            playlist.PlaylistSongs.Add(playlistSongController.AddSongToPlaylist(playlist, song2.ID));
            playlist.PlaylistSongs.Add(playlistSongController.AddSongToPlaylist(playlist, song3.ID));

            playlist.PlaylistSongs.ToList().ForEach(x =>
            {
                x.Song = songController.GetItem(x.SongID);
            });
            
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
            for (int i = 0; i < playlist.PlaylistSongs.Count; i++)
                if (playlist.PlaylistSongs.ToList()[i].Song.ID != AudioPlayer.Instance.Queue[i].ID) areEqual = false;

            Assert.IsTrue(areEqual);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Next_NextPlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);
            AudioPlayer.Instance.Next();

            Assert.AreEqual(playlist.PlaylistSongs.ToList()[1].SongID, AudioPlayer.Instance.CurrentSong.ID);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Loop_SongQueue_ContainsSongs()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist);
            AudioPlayer.Instance.Loop();

            Assert.IsTrue(playlist.PlaylistSongs.Count * 2 == AudioPlayer.Instance.Queue.Count);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Loop_ContinuePlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist, 1);
            AudioPlayer.Instance.Loop();
            AudioPlayer.Instance.Next();

            Assert.AreEqual(playlist.PlaylistSongs.ToList()[0].SongID, AudioPlayer.Instance.CurrentSong.ID);
        }

        [Test, Category("Local")]
        public void AudioPlayer_Previous_PreviousPlaying()
        {
            AudioPlayer.Instance.PlayPlaylist(playlist, 0);
            AudioPlayer.Instance.Prev();
            
            Assert.AreEqual(playlist.PlaylistSongs.ToList()[0].SongID, AudioPlayer.Instance.CurrentSong.ID);

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

            Assert.IsFalse(AudioPlayer.Instance.Queue[1].Equals(playlist.PlaylistSongs.ToList()[1].Song));
        }

        [TearDown, Category("Local")]
        public void TearDown()
        {
            playlistSongController.RemoveFromPlaylist(playlist, song.ID);
            playlistSongController.RemoveFromPlaylist(playlist, song2.ID);
            playlistSongController.RemoveFromPlaylist(playlist, song3.ID);
            playlistController.DeleteItem(playlist.ID);
            songController.DeleteItem(song.ID);
            songController.DeleteItem(song2.ID);
            songController.DeleteItem(song3.ID);
            artistController.DeleteItem(song.Artist.ID);
        }
    }
}