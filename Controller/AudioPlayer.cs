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

        /*private static int _currentSongIndex = -1;*/
        public static int CurrentSongIndex
        {
            get; set;
        }

        public static List<Song> SongQueue { get; set; } = new List<Song>();
        public static bool _looping = false;

        public static void Initialize()
        {
            WaveOutDevice = new WaveOut();
            WaveOutDevice.Volume = 0.05f;
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
            ClearSongQueue();
            var songs = new PlaylistSongController(new Model.Data.DatabaseContext()).GetSongsFromPlaylist(playlist.ID);
            CurrentSongIndex = -1;

            for (int i = startIndex; i < songs.Count; i++)
            {
                AddSong(songs[i].Song);
            }

            if (_looping)
            {
                for (int i = 0; i < startIndex; i++)
                {
                    AddSong(songs[i].Song);
                }
            }

            Next();
        }
    }
}
