using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SongInfo
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime Added { get; set; }

        public SongInfo(string title, string artist, TimeSpan duration, DateTime added)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            Added = added;
        }
    }
}
