using System.Collections.Generic;
using Model;
using NAudio.Wave;

namespace Controller
{
    public static class AudioPlayer
    {
        public static IWavePlayer WaveOutDevice { get; set; }
        public static SongAudioFile CurrentSong { get; set; }

        private static int _currentSongIndex = -1;
        private static int CurrentSongIndex
        {
            get => _currentSongIndex;
            set
            {
                if (SongQueue.Count == 0)
                    _currentSongIndex = 0;

                _currentSongIndex = value;

                if (_looping)
                {
                    if (_currentSongIndex < 0)
                        _currentSongIndex = SongQueue.Count - 1;
                    if (_currentSongIndex >= SongQueue.Count)
                        _currentSongIndex = 0;
                }
                else 
                {
                    if (_currentSongIndex < 0)
                        _currentSongIndex = 0;
                    if (_currentSongIndex >= SongQueue.Count)
                        _currentSongIndex = SongQueue.Count - 1;
                }
                
            }
        }

        public static List<SongAudioFile> SongQueue { get; set; } = new List<SongAudioFile>();
        public static bool _looping = false;

        public static void Initialize()
        {
            WaveOutDevice = new WaveOut();
            WaveOutDevice.Volume = 1.0f;
        }

        public static void Next()
        {
            CurrentSong = SongQueue[++CurrentSongIndex];
            if (CurrentSong != null)
                WaveOutDevice.Init(CurrentSong.AudioFile);
        }

        public static void Prev()
        {
            CurrentSong = SongQueue[--CurrentSongIndex];
            if (CurrentSong != null)
                WaveOutDevice.Init(CurrentSong.AudioFile);
        }

        public static void Loop()
        {
            _looping = !_looping;
        }

        public static void Shuffle()
        {

        }

        public static void PlaySong(SongAudioFile song)
        {
            AddSong(song);
            CurrentSongIndex = SongQueue.Count;
            CurrentSong = song;
            WaveOutDevice.Init(CurrentSong.AudioFile);
            WaveOutDevice.Play();
        }

        public static void AddSong(SongAudioFile song)
        {
            SongQueue.Add(song);
        }
    }
}
