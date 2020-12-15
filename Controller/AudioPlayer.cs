using System;
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
        public string CurrentSongArtistName { get; set; }
        public double MaxVolume { get; set; }
        public event EventHandler NextSong;

        public int CurrentSongIndex  { get; set; }

        public List<Song> Queue { get; set; } = new List<Song>();
        public List<Song> NextInQueue { get; set; } = new List<Song>();
        public bool Looping { get; set; } = false;

        public static AudioPlayer Instance { get; set; }

        
        public static AudioPlayer Create(IDatabaseContext context)
        {
            var x = ProxyController.AddToProxy<AudioPlayer>(context);
            x.Initialize();
            Instance = x;
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
        [HasPermission(Permission = Permissions.SongNext)]
        public virtual void Next()
        {
            if(CurrentSongIndex >= 0)
            {
                var previousSong = Queue[CurrentSongIndex];

                if (NextInQueue.Contains(previousSong))
                    NextInQueue.Remove(previousSong);

                if (Looping)
                    AddSongToQueue(previousSong);
            }
            
            if (Queue.Count == 0) return;
            CurrentSongIndex++;

            if (CurrentSongIndex >= Queue.Count)
                CurrentSongIndex--;

            PlaySong(Queue[CurrentSongIndex]);
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
                {
                    CurrentSongIndex = 0;
                }
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
            if(CurrentSong != null)
                WaveOutDevice.Stop();
            CurrentSongFile = new SongAudioFile(FileCache.Instance.GetFile(song.Path));
            CurrentSong = song;
            CurrentSongArtistName = ArtistController.Create(new DatabaseContext()).GetItem(song.Artist).ArtistName;
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
            PlayPlaylist(PlaylistSongController.Create(new DatabaseContext()).GetSongsFromPlaylist(playlist.ID), startIndex);
        }

        public void PlayPlaylist(List<PlaylistSong> songs, int startIndex = -1)
        {
            ClearQueue();
            CurrentSongIndex = -1;

            NextInQueue.ForEach(i => AddSongToQueue(i));
            songs.Where(song => song.Index > startIndex).ToList().ForEach(i => AddSongToQueue(i.Song));

            if (Looping)
            {
                songs.ForEach(i => AddSongToQueue(i.Song));
            }

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
        public virtual void Loop(Playlist playlist)
        {
            Looping = !Looping;

            if (Looping && playlist != null)
            {
                var songs = PlaylistSongController.Create(new DatabaseContext()).GetSongsFromPlaylist(playlist.ID);
                songs.ForEach(i => AddSongToQueue(i.Song));
            }
            else if(!Looping)
            {
                Queue = Queue.GroupBy(p => p.ID).Select(g => g.First()).ToList();
            }
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
