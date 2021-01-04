﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public List<Song> CurrentAlbumSongs { get; set; } = new List<Song>();
        public string CurrentSongArtistName { get; set; }
        public double MaxVolume { get; set; }
        public event EventHandler NextSong;

        public int CurrentSongIndex  { get; set; }

        public List<Song> Queue { get; set; } = new List<Song>();
        public List<Song> NextInQueue { get; set; } = new List<Song>();
        public bool Looping { get; set; }
        public bool Shuffling { get; set; }

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

        public void ResetAudioPlayer()
        {
            WaveOutDevice.Stop();
            Create(DatabaseContext.Instance);
        }

        /**
         * Skips to the next song
         *
         * @return void
         */
        public void Next()
        {
            if (CurrentSongIndex >= 0 && CurrentSong != null)
                if (NextInQueue.Contains(Queue[CurrentSongIndex]))
                    NextInQueue.Remove(Queue[CurrentSongIndex]);
                else if (Looping)
                    AddSongToQueue(CurrentSong);
            
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
                    copyQueue.ForEach(AddSongToQueue);
                }
                else
                    CurrentSongIndex = 0;
            }

            PlaySong(Queue[CurrentSongIndex]);
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
            CurrentSongFile.AudioFile.CurrentTime = TimeSpan.FromSeconds(0);
            CurrentSong = song;
            CurrentSongArtistName = song.Artist.ArtistName;
            WaveOutDevice.Init(CurrentSongFile.AudioFile);
            NextSong?.Invoke(this, new EventArgs());
            Task.Delay(500).ContinueWith(x => WaveOutDevice.Play());
        }

        /**
         * Adds a song to the Queue
         *
         * @param song The song that has to be played in the song queue
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
         * @param song The song that has to be added to the song queue
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
         * @param startIndex Gets the corresponding starting index
         *
         * @return void
         */
        public void PlayPlaylist(Playlist playlist, int startIndex = -1)
        {
            CurrentPlaylist = playlist;
            CurrentAlbumSongs.Clear();

            PlayQueue(startIndex);
        }

        /**
         * Plays an album
         *
         * @param albumSongs Which album needs to be played
         * @param startIndex Gets the corresponding starting index
         *
         * @return void
         */
        public void PlayAlbum(List<Song> albumSongs, int startIndex = -1)
        {
            CurrentPlaylist = null;
            CurrentAlbumSongs = albumSongs;

            PlayQueue(startIndex);
        }

        /**
         * Plays the songs in the queue
         *
         * @param index Gets the corresponding starting index
         *
         * @return void
         */
        public void PlayQueue(int startIndex = -1)
        {
            ClearQueue();

            if (NextInQueue.Count > 0)
            {
                NextInQueue.ForEach(AddSongToQueue);
                CurrentSongIndex = -1;
            }
            else
                CurrentSongIndex = startIndex;

            FillQueue();
            Next();
        }

        /**
         * Keeps playing the playlists over and over again
         *
         * @return void
         */
        [HasPermission(Permission = Permissions.SongLoop)]
        public virtual void Loop()
        {
            Looping = !Looping;
            FillQueue();
        }

        /**
         * Shuffles the playlist
         *
         * @return void
         */
        [HasPermission(Permission = Permissions.SongShuffle)]
        public virtual void Shuffle()
        {
            Shuffling = !Shuffling;
            FillQueue();
        }

        /**
         * Fills the queue with the songs in the playlist
         *
         * @return void
         */
        private void FillQueue()
        {
            var songs = new List<Song>();

            if (CurrentPlaylist != null)
                songs = CurrentPlaylist.PlaylistSongs?.Select(x => x.Song).ToList() ?? new List<Song>();

            if (CurrentAlbumSongs.Count > 0)
                songs = CurrentAlbumSongs;

            if (songs.Count > 0)
            {
                var queueFromCurrentSongIndex = new List<Song>(songs);

                if (Queue.Count > NextInQueue.Count)
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
                        songs.Where(x => !queueFromCurrentSongIndex.Contains(x)).ToList().ForEach(i => queueFromCurrentSongIndex.Add(i));
                        tempList.ForEach(i => queueFromCurrentSongIndex.Add(i));
                    }
                    else
                    {
                        var tempList = new List<Song>(queueFromCurrentSongIndex);
                        queueFromCurrentSongIndex.Clear();
                        songs.Where(x => tempList.Contains(x)).ToList().ForEach(i => queueFromCurrentSongIndex.Add(i));
                        songs.ForEach(i => queueFromCurrentSongIndex.Add(i));
                    }
                }
                else
                {
                    if (Shuffling)
                        queueFromCurrentSongIndex = ShuffleList(queueFromCurrentSongIndex);
                    else
                    {
                        var tempList = new List<Song>(queueFromCurrentSongIndex);
                        queueFromCurrentSongIndex.Clear();
                        songs.Where(x => tempList.Contains(x)).ToList().ForEach(i => queueFromCurrentSongIndex.Add(i));
                    }
                }

                if(Queue.Count > 0)
                {
                    if(NextInQueue.Count > 0 && NextInQueue[0] == CurrentSong)
                        Queue.RemoveRange(CurrentSongIndex + NextInQueue.Count, Queue.Count - (CurrentSongIndex + NextInQueue.Count));
                    else
                        Queue.RemoveRange(CurrentSongIndex + 1 + NextInQueue.Count, Queue.Count - (CurrentSongIndex + 1 + NextInQueue.Count));
                }
                    
                
                Queue.AddRange(queueFromCurrentSongIndex);
            }
        }

        /**
         * Shuffles a list of songs
         *
         * @param songs Which list needs to be shuffled
         *
         * @return List<Song> : The shuffled list
         */
        private List<Song> ShuffleList(List<Song> songs)
        {
            if (songs.Count <= 1)
                return songs;

            List<Song> shuffledList;
            var random = new Random();

            while (true)
            {
                shuffledList = songs.OrderBy(i => random.Next()).ToList();

                if (!shuffledList[0].Equals(songs[0]))
                    break;
            }

            return shuffledList;
        }

        /**
         * Plays and pauses a song
         *
         * @return void
         */
        public void PlayPause()
        {
            if(CurrentSong == null) return;
            
            if (WaveOutDevice.PlaybackState == PlaybackState.Paused || WaveOutDevice.PlaybackState == PlaybackState.Stopped)
                WaveOutDevice.Play();
            else
                WaveOutDevice.Pause();
        }

        /**
         * Changes the maximum volume
         *
         * @param selectedItem Gets loudness option
         *
         * @return void
         */
        public void ChangeVolume(int selectedItem)
        {
            switch (selectedItem)
            {
                case 0:
                    MaxVolume = 0.1;
                    break;
                case 1:
                    MaxVolume = 0.2;
                    break;
                case 2:
                    MaxVolume = 0.4;
                    break;
                default:
                    break;
            }
        }
    }
}