using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller
{
    public class QueueController
    {
        private static QueueController instance;
        public static QueueController Instance
        {
            get
            {
                if (instance == null)
                    instance = new QueueController();
                return instance;
            }
        }

        public void DeleteSongFromQueue(Song song)
        {
            AudioPlayer.Instance.Queue.Remove(song);
            AudioPlayer.Instance.NextInQueue.Remove(song);
        }

        public void SwapSongs(int indexOne, int indexTwo, List<Song> list)
        {
            var listitem1 = list[indexOne];
            var listitem2 = list[indexTwo];

            list[indexOne] = listitem2;
            list[indexTwo] = listitem1;
        }

    }
}
