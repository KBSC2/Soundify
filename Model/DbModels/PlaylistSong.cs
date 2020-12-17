using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class PlaylistSong
    {
        public int SongID { get; set; }
        [ForeignKey("SongID")] public virtual Song Song { get; set; }

        public int PlaylistID { get; set; }

        public virtual Playlist Playlist { get; set; }

        public DateTime Added { get; set; }

        public int Index { get; set; }
    }
}