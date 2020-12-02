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

        public List<SongInfo> NextInQueueItems => SongInfo.ConvertSongListToSongInfo(AudioPlayer.Instance.NextInQueue);
        public List<SongInfo> NextInPlaylistItems => QueueGenerator(AudioPlayer.Instance.NextInPlaylist, AudioPlayer.Instance.CurrentSongIndex);
        public string CurrentSongName => AudioPlayer.Instance.CurrentSong == null ? "" : AudioPlayer.Instance.CurrentSong.Name;
        public string CurrentSongArtist => AudioPlayer.Instance.CurrentSong == null ? "" : AudioPlayer.Instance.CurrentSong.Artist;
        public string CurrentSongDuration => AudioPlayer.Instance.CurrentSong == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSong.Duration).ToString("m':'ss");
        public string NextInQueueLabelContent => AudioPlayer.Instance.NextInQueue.Count == 0 ? "" : "Next in queue";
        public string NextInPlaylistLabelMargin => "35," + (120 + (AudioPlayer.Instance.NextInQueue.Count * 30)).ToString() + ",0,0";
        public string NextInPlaylistMargin => "300," + (130 + (AudioPlayer.Instance.NextInQueue.Count * 30)).ToString() + ",0,0";

        public List<SongInfo> QueueGenerator(List<Song> songs, int index)
        {
            List<Song> queue = new List<Song>();

            for(int i = index+1; i < songs.Count; i++)
            {
                queue.Add(songs[i]);
                if (queue.Count > 12) break;
            }

            if (AudioPlayer.Instance.Looping)
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
