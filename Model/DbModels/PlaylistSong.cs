using System;

namespace Model.DbModels
{
    public class PlaylistSong
    {

        public int SongID { get; set; }
        public virtual Song Song { get; set; }

        public int PlaylistID { get; set; }
        public virtual Playlist Playlist { get; set; }
        public DateTime Added { get; set; }
        public int Index { get; set; }
    }
}
