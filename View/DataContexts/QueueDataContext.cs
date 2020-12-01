using Controller;
using Model.Annotations;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace View.DataContexts
{
    public class QueueDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static QueueDataContext _instance;
        public static QueueDataContext Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new QueueDataContext();
                return _instance;
            }
        }

        public List<SongInfo> QueueItems => QueueGenerator(AudioPlayer.SongQueue, AudioPlayer.CurrentSongIndex);
        public string CurrentSongName => AudioPlayer.CurrentSong == null ? "" : AudioPlayer.CurrentSong.Name;
        public string CurrentSongArtist => AudioPlayer.CurrentSong == null ? "" : AudioPlayer.CurrentSong.Artist;
        public string CurrentSongDuration => AudioPlayer.CurrentSong == null ? "" : TimeSpan.FromSeconds(AudioPlayer.CurrentSong.Duration).ToString("m':'ss");

        public List<SongInfo> QueueGenerator(List<Song> songs, int index)
        {
            List<Song> queue = new List<Song>();

            for(int i = index+1; i < songs.Count; i++)
            {
                queue.Add(songs[i]);
                if (queue.Count > 12) break;
            }

            if (AudioPlayer.Looping)
            {
                for (int i = 0; i < index; i++)
                {
                    queue.Add(songs[i]);
                    if (queue.Count > 12) break;
                }

                for (int i = index; i < songs.Count; i++)
                {
                    queue.Add(songs[i]);
                    if (queue.Count > 12) break;
                }
            }

            return SongInfo.ConvertSongListToSongInfo(queue);
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
