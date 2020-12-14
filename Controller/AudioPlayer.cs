﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public Playlist CurrentPlaylist { get; set; }
        public double MaxVolume { get; set; }
        public event EventHandler NextSong;

        public int CurrentSongIndex  { get; set; }

        public List<Song> Queue { get; set; } = new List<Song>();
        public List<Song> NextInQueue { get; set; } = new List<Song>();
        public bool Looping { get; set; } = false;
        public bool Shuffling { get; set; } = false;

        public static AudioPlayer Instance { get; set; }
        public static IDatabaseContext Context { get; set; }

        public static AudioPlayer Create(IDatabaseContext context)
        {
            var x = ProxyController.AddToProxy<AudioPlayer>(context);
            x.Initialize();
            Instance = x;
            Context = context;
            return x;
        }

        /**
         * Initializes the waveOut device
         *
         * @return void
         */
        public void Initialize()
        {
            WaveOutDevice = new WaveOutEvent { Volume = 0.05f };
            MaxVolume = 0.2;
            CurrentSongIndex = -1;
        }

        /**
         * Skips to the next song
         *
         * @return void
         */
        public void Next()
        {
            FillQueue(CurrentPlaylist);
            if (CurrentSongIndex >= 0 && CurrentSong != null)
                if (NextInQueue.Contains(Queue[CurrentSongIndex]))
                    NextInQueue.Remove(Queue[CurrentSongIndex]);
            
            if (Queue.Count == 0) return;
            CurrentSongIndex++;

            if (CurrentSongIndex >= Queue.Count)
                CurrentSongIndex--;

            PlaySong(Queue[CurrentSongIndex]);
        }

        [HasPermission(Permission = Permissions.SongNext)]
        public virtual void NextButton()
        {
            Next();
        }

        /**
         * Goes back to the previous song
         *
         * @return void
         */
        [HasPermission(Permission = Permissions.SongPrev)]
        public virtual void Prev()
        {
            if (Queue.Count == 0) return;
            CurrentSongIndex--;
            if (CurrentSongIndex < 0)
            {
                if (Looping)
                {
                    CurrentSongIndex = Queue.Count - 1;
                    List<Song> copyQueue = new List<Song>(Queue);
                    copyQueue.ForEach(i => AddSongToQueue(i));
                }
                else
                    CurrentSongIndex = 0;
            }

            PlaySong(Queue[CurrentSongIndex]);
            FillQueue(CurrentPlaylist);
        }

        /**
         * Plays the current selected song
         *
         * @param song The song that has to be played
         *
         * @return void
         */
        public void PlaySong(Song song)
        {
            if (CurrentSong != null)
                WaveOutDevice.Stop();
            CurrentSongFile = new SongAudioFile(FileCache.Instance.GetFile(song.Path));
            CurrentSong = song;
            WaveOutDevice.Init(CurrentSongFile.AudioFile);
            NextSong?.Invoke(this, new EventArgs());
            Task.Delay(500).ContinueWith(x => WaveOutDevice.Play());
        }

        /**
         * Adds a song to the Queue
         *
         * @param song The song that has to be played
         *
         * @return void
         */
        public void AddSongToQueue(Song song)
        {
            Queue.Add(song);
        }

        /**
         * Adds a song to the songQueue
         *
         * @param song The song that has to be played
         *
         * @return void
         */
        public void AddSongToSongQueue(Song song)
        {
            Queue.Insert(CurrentSongIndex+1, song);

            if (CurrentSong == null)
            {
                CurrentSongIndex = 0;
                PlaySong(song);
            }
            else
                NextInQueue.Add(song);
        }
        
        /**
         * Clears the Queue
         *
         * @return void
         */
        public void ClearQueue()
        {
            Queue.Clear();
        }

        /**
         * Plays a playlist
         *
         * @param playlist Gets the playlist 
         * @param index Gets the corresponding index
         *
         * @return void
         */
        public void PlayPlaylist(Playlist playlist, int startIndex = -1)
        {
            CurrentPlaylist = playlist;
            ClearQueue();
            if(NextInQueue.Count > 0)
            {
                NextInQueue.ForEach(i => AddSongToQueue(i));
                CurrentSongIndex = -1;
            }
            else
                CurrentSongIndex = startIndex;

            Next();
        }

        /**
         * Keeps playing the playlists over and over again
         *
         * @param playlist Which playlist needs to be looped over
         *
         * @return void
         */
        [HasPermission(Permission = Permissions.SongLoop)]
        public virtual void Loop()
        {
            Looping = !Looping;
            FillQueue(CurrentPlaylist);
        }

        [HasPermission(Permission = Permissions.SongShuffle)]
        public virtual void Shuffle()
        {
            Shuffling = !Shuffling;
            FillQueue(CurrentPlaylist);
        }

        private void FillQueue(Playlist playlist)
        {
            if (playlist != null)
            {
                var songs = PlaylistSongController.Create(Context).GetSongsFromPlaylist(playlist.ID);

                var queueFromCurrentSongIndex = songs.Select(i => i.Song).ToList();

                if (Queue.Count > 0)
                    queueFromCurrentSongIndex = Queue.GetRange(CurrentSongIndex + 1, Queue.Count - (CurrentSongIndex + 1));
                
                NextInQueue.ForEach(x => queueFromCurrentSongIndex.Remove(x));

                if (queueFromCurrentSongIndex.Count > songs.Count)
                    queueFromCurrentSongIndex.RemoveRange(queueFromCurrentSongIndex.Count - songs.Count, songs.Count);

                if (Looping)
                {
                    if (Shuffling)
                    {
                        queueFromCurrentSongIndex = ShuffleList(queueFromCurrentSongIndex);
                        var tempList = new List<Song>(queueFromCurrentSongIndex);
                        songs.Where(x => !queueFromCurrentSongIndex.Contains(x.Song)).Select(x => x.Song).ToList().ForEach(i => queueFromCurrentSongIndex.Add(i));
                        tempList.ForEach(i => queueFromCurrentSongIndex.Add(i));
                    }
                    else
                        songs.ForEach(i => queueFromCurrentSongIndex.Add(i.Song));
                }
                else
                {
                    if (Shuffling)
                        queueFromCurrentSongIndex = ShuffleList(queueFromCurrentSongIndex);
                    else
                    {
                        var tempList = new List<Song>(queueFromCurrentSongIndex);
                        queueFromCurrentSongIndex.Clear();
                        songs.Where(x => tempList.Contains(x.Song)).Select(x => x.Song).ToList().ForEach(i => queueFromCurrentSongIndex.Add(i));
                    }
                }

                if(Queue.Count > 0)
                    Queue.RemoveRange(CurrentSongIndex + 1 + NextInQueue.Count, Queue.Count - (CurrentSongIndex + 1 + NextInQueue.Count));
                
                Queue.AddRange(queueFromCurrentSongIndex);
            }
        }

        private List<Song> ShuffleList(List<Song> songs)
        {
            var shuffledList = new List<Song>(songs);
            var random = new Random();

            while (true)
            {
                shuffledList = songs.OrderBy(i => random.Next()).ToList();

                if (!shuffledList[0].Equals(songs[0]))
                    break;
            }

            return shuffledList;
        }

        public void PlayPause()
        {
            if (WaveOutDevice.PlaybackState == PlaybackState.Paused || WaveOutDevice.PlaybackState == PlaybackState.Stopped)
                WaveOutDevice.Play();
            else
                WaveOutDevice.Pause();
        }

        public void ChangeVolume(int selectedItem)
        {
            MaxVolume = selectedItem switch
            {
                0 => 0.1,
                1 => 0.2,
                2 => 0.4,
                _ => 0.0
            };
        }
    }
}