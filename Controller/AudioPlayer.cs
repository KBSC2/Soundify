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
        public IWavePlayer WaveOutDevice { get; set; }
        public SongAudioFile CurrentSongFile { get; set; }
        public Song CurrentSong { get; set; }
        public double MaxVolume { get; set; }
        public event EventHandler NextSong;

        public int CurrentSongIndex  { get; set; }

        public List<Song> NextInQueue { get; set; } = new List<Song>();
        public List<Song> NextInPlaylist { get; set; } = new List<Song>();
        public bool Looping { get; set; } = false;

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
            WaveOutDevice = new WaveOut { Volume = 0.05f };
            MaxVolume = 0.2;
        }

        [HasPermission(Permission = Permissions.SongNext)]
        public void Next()
        {
            if(NextInQueue.Count > 0)
            {
                PlaySong(NextInQueue[0]);
                NextInQueue.RemoveAt(0);
            } 
            else
            {
                if (NextInPlaylist.Count == 0) return;
                CurrentSongIndex++;
                if (CurrentSongIndex >= NextInPlaylist.Count)
                    CurrentSongIndex = Looping ? 0 : CurrentSongIndex - 1;

                PlaySong(NextInPlaylist[CurrentSongIndex]);
            }
        }

        [HasPermission(Permission = Permissions.SongPrev)]
        public void Prev()
        {
            if (NextInPlaylist.Count == 0) return;
            CurrentSongIndex--;
            if (CurrentSongIndex < 0)
                CurrentSongIndex = Looping ? NextInPlaylist.Count - 1 : 0;

            PlaySong(NextInPlaylist[CurrentSongIndex]);
        }

        public void PlaySong(Song song)
        {
            CurrentSongFile = new SongAudioFile(FileCache.Instance.GetFile(song.Path));
            CurrentSong = song;
            WaveOutDevice.Init(CurrentSongFile.AudioFile);
            NextSong?.Invoke(this, new EventArgs());
            //TODO:why is there a delay here?
            Task.Delay(500).ContinueWith(x => WaveOutDevice.Play());
        }

        public void AddSongToPlaylistQueue(Song song)
        {
            NextInPlaylist.Add(song);
        }

        public void AddSongToSongQueue(Song song)
        {
            NextInQueue.Add(song);
        }

        private void ClearSongQueue()
        {
            NextInPlaylist.Clear();
        }

        public void PlayPlaylist(Playlist playlist, int startIndex = -1)
        {
            PlayPlaylist(PlaylistSongController.Create(new DatabaseContext()).GetSongsFromPlaylist(playlist.ID), startIndex);
        }

        public void PlayPlaylist(List<PlaylistSong> songs, int startIndex = -1)
        {
            ClearSongQueue();
            CurrentSongIndex = startIndex;

            songs.ForEach(i => AddSongToPlaylistQueue(i.Song));

            Next();
        }
    }
}
