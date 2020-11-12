using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DbModels
{
    public class PlaylistSong
    {

        public int SongID { get; set; }
        public Song Song { get; set; }

        public int PlaylistID { get; set; }
        public Playlist Playlist { get; set; }
    }
}
