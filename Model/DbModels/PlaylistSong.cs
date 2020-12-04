using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class PlaylistSong
    {

        [ForeignKey("Song")]
        public int SongID { get; set; }
        public virtual Song Song { get; set; }
        [ForeignKey("Playlist")]
        public int PlaylistID { get; set; }
        public virtual Playlist Playlist{ get; set; }
        public DateTime Added { get; set; }
        public int Index { get; set; }
    }
}
