using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DbModels
{
    public class Playlist : DbModel 
    {
        public IList<PlaylistSong> PlaylistSongs { get; set; }
    }
}
