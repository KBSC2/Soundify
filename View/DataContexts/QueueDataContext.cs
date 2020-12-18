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

        private static QueueDataContext instance;
        public static QueueDataContext Instance => instance ??= new QueueDataContext();

        public List<SongInfo> NextInQueueItems => QueueGenerator(AudioPlayer.Instance.Queue, AudioPlayer.Instance.CurrentSongIndex); //TODO: Kan dit weg?
        public string CurrentSongName => AudioPlayer.Instance.CurrentSong?.Name ?? "";
        public string CurrentSongArtist => AudioPlayer.Instance.CurrentSong?.Artist.ArtistName ?? "";
        public string CurrentSongDuration => AudioPlayer.Instance.CurrentSong == null ? "" : TimeSpan.FromSeconds(AudioPlayer.Instance.CurrentSong.Duration).ToString("m':'ss");

        public List<SongInfo> QueueGenerator(List<Song> songs, int index)
        {
            var queue = new List<Song>();

            for(var i = index+1; i < songs.Count; i++)
            {
                queue.Add(songs[i]);
                if (queue.Count > 12) break;
            }

            return SongInfo.ConvertSongListToSongInfo(queue);
        }

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
