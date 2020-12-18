using System;


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
            Duration = durationTimeSpan;
            File = file;
        }
    }
}
