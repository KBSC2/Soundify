using Controller;
using Model.Annotations;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Controller.DbControllers;
using Model.Database.Contexts;

namespace View.DataContexts
{
    public class QueueDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static QueueDataContext instance;
        public static QueueDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new QueueDataContext();
                return instance;
            }
        }

        public List<SongInfo> NextInQueueItems => QueueGenerator(AudioPlayer.Instance.Queue, AudioPlayer.Instance.CurrentSongIndex);
        public string CurrentSongName => AudioPlayer.Instance.CurrentSong == null ? "" : AudioPlayer.Instance.CurrentSong.Name;
        public string CurrentSongArtist => AudioPlayer.Instance.CurrentSong == null ? ""
            : ArtistController.Create(DatabaseContext.Instance).GetItem(AudioPlayer.Instance.CurrentSong.ArtistID).ArtistName;
        public string CurrentSongDuration => AudioPlayer.Instance.CurrentSong == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSong.Duration).ToString("m':'ss");

        public List<SongInfo> QueueGenerator(List<Song> songs, int index)
        {
            List<Song> queue = new List<Song>();

            for(int i = index+1; i < songs.Count; i++)
            {
                queue.Add(songs[i]);
                if (queue.Count > 12) break;
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
