using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace View.ListItems
{
    public class AlbumSongInfo
    {
        public string Title { get; set; }
        public string ProducedBy { get; set; }
        public string WrittenBy { get; set; }
        public TimeSpan Duration { get; set; }

        public TagLib.File File { get; set; }

        public AlbumSongInfo(string title, TimeSpan durationTimeSpan, TagLib.File file)
        {
            Title = title;
            ProducedBy = "";
            WrittenBy = "";
            Duration = durationTimeSpan;
            File = file;
        }
    }
}
