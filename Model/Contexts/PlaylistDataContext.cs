using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class PlaylistDataContext
    {
        public string PlaylistName { get; set; }
        public string Description { get; set; }
        public List<SongInfo> PlaylistItems { get; set; }

        public PlaylistDataContext()
        {
            PlaylistName = "Test playlist";
            Description = "a description for the test playlist";

            PlaylistItems = new List<SongInfo>
            {
                new SongInfo("test nummer 1", "een artiest", "06:66", "today"),
                new SongInfo("test nummer 2", "nog een artiest", "04:20", "today"),
                new SongInfo("test nummer 3", "een andere artiest", "00:00", "today"),
                new SongInfo("test nummer 4", "dezelfde artiest", "12:34", "today"),
            };
        }
    }
}
