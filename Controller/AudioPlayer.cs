using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Controller.DbControllers;
using Controller.Proxy;
using Model;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NAudio.Wave;

namespace Controller
{
    public class AudioPlayer
    {
        public static IWavePlayer WaveOutDevice { get; set; }
        public static SongAudioFile CurrentSongFile { get; set; }
        public static Song CurrentSong { get; set; }
        public static double MaxVolume { get; set; }
        public static event EventHandler NextSong;

        public static int CurrentSongIndex  { get; set; }

        public static List<Song> SongQueue { get; set; } = new List<Song>();
        public static bool Looping { get; set; } = false;

        public static AudioPlayer Instance { get; set; }


        public static AudioPlayer Create(IDatabaseContext context)
        {
            var x = ProxyController.AddToProxy<AudioPlayer>(context);
            x.Initialize();
            Instance = x;
            return x;
        }

        public void Initialize()
        {
            WaveOutDevice = new WaveOut();
            WaveOutDevice.Volume = 0.05f;
            MaxVolume = 0.2;
        }

        [HasPermission(Permission = Permissions.SongNext)]
        public void Next()
        {
            if (SongQueue.Count == 0) return;
            CurrentSongIndex++;
            if (CurrentSongIndex >= SongQueue.Count)
                CurrentSongIndex = Looping ? 0 : CurrentSongIndex - 1;

            PlaySong(SongQueue[CurrentSongIndex]);
        }

        public void Prev()
        {
            if (SongQueue.Count == 0) return;
            CurrentSongIndex--;
            if (CurrentSongIndex < 0)
                CurrentSongIndex = Looping ? SongQueue.Count - 1 : 0;

            PlaySong(SongQueue[CurrentSongIndex]);
        }

        public void PlaySong(Song song)
        {
            CurrentSongFile = new SongAudioFile(FileCache.Instance.GetFile(song.Path));
            CurrentSong = song;
            WaveOutDevice.Init(CurrentSongFile.AudioFile);
            NextSong?.Invoke(null, new EventArgs());
            Task.Delay(500).ContinueWith(x => WaveOutDevice.Play());
        }

        public void AddSong(Song song)
        {
            SongQueue.Add(song);
        }


        private void ClearSongQueue()
        {
            SongQueue.Clear();
        }

        public void PlayPlaylist(Playlist playlist, int startIndex = -1)
        {
            PlayPlaylist(PlaylistSongController.Create(new Model.Database.Contexts.DatabaseContext()).GetSongsFromPlaylist(playlist.ID), startIndex);
        }

        public void PlayPlaylist(List<PlaylistSong> songs, int startIndex = -1)
        {
            ClearSongQueue();
            CurrentSongIndex = startIndex;

            songs.ForEach(i => AddSong(i.Song));

            Next();
        }
    }
}
