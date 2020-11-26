using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controller.DbControllers;
using Model;
using Model.DbModels;
using NAudio.Wave;

namespace Controller
{
    public static class AudioPlayer
    {
        public static IWavePlayer WaveOutDevice { get; set; }
        public static SongAudioFile CurrentSong { get; set; }
        public static double MaxVolume { get; set; }
        public static event EventHandler NextSong;

        public static int CurrentSongIndex  { get; set; }

        public static List<Song> SongQueue { get; set; } = new List<Song>();
        public static bool _looping = false;

        public static void Initialize()
        {
            WaveOutDevice = new WaveOut();
            WaveOutDevice.Volume = 0.05f;
            MaxVolume = 0.2;
        }

        public static void Next()
        {
            if (SongQueue.Count == 0) return;
            CurrentSongIndex++;
            if (CurrentSongIndex >= SongQueue.Count)
                CurrentSongIndex = _looping ? 0 : CurrentSongIndex - 1;

            PlaySong(SongQueue[CurrentSongIndex]);
        }

        public static void Prev()
        {
            if (SongQueue.Count == 0) return;
            CurrentSongIndex--;
            if (CurrentSongIndex < 0)
                CurrentSongIndex = _looping ? SongQueue.Count - 1 : 0;

            PlaySong(SongQueue[CurrentSongIndex]);
        }

        public static void Loop()
        {
            _looping = !_looping;
        }

        public static void Shuffle()
        {

        }

        public static void PlaySong(Song song)
        {
            NextSong?.Invoke(null, new EventArgs());
            CurrentSong = new SongAudioFile(FileCache.Instance.GetFile(song.Path));
            WaveOutDevice.Init(CurrentSong.AudioFile);
            Task.Delay(500).ContinueWith(x => WaveOutDevice.Play());
        }

        public static void AddSong(Song song)
        {
            SongQueue.Add(song);
        }


        private static void ClearSongQueue()
        {
            SongQueue.Clear();
        }

        public static void PlayPlaylist(Playlist playlist, int startIndex = 0)
        {
            PlayPlaylist(new PlaylistSongController(new Model.Database.Contexts.DatabaseContext()).GetSongsFromPlaylist(playlist.ID), startIndex);
        }

        public static void PlayPlaylist(List<PlaylistSong> songs, int startIndex = 0)
        {
            ClearSongQueue();
            CurrentSongIndex = startIndex;

            foreach(PlaylistSong playlistSong in songs)
            {
                AddSong(playlistSong.Song);
            }

            Next();
        }
    }
}
