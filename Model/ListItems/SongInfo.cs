using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SongInfo
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Duration { get; set; }
        public string Added { get; set; }

        public SongInfo(string title, string artist, string duration, string added)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            Added = added;
        }
    }
}
