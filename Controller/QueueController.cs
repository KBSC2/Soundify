using Model.DbModels;
using System.Collections.Generic;

namespace Controller
{
    public class QueueController
    {
        private static QueueController instance;
        public static QueueController Instance => instance ??= new QueueController();

        public void DeleteSongFromQueue(Song song)
        {
            AudioPlayer.Instance.Queue.Remove(song);
            AudioPlayer.Instance.NextInQueue.Remove(song);
        }

        public void SwapSongs(int indexOne, int indexTwo, List<Song> list)
        {
            var listItem1 = list[indexOne];
            var listItem2 = list[indexTwo];

            list[indexOne] = listItem2;
            list[indexTwo] = listItem1;
        }

    }
}
