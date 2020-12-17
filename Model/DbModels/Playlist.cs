using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class Playlist : DbModel 
    {
        [Required] public string Name { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public DateTime CreationDate { get; set; }
        
        [Required] public int UserID { get; set; }
        public virtual User User { get; set; }
        
        [Required] public bool ActivePlaylist { get; set; }
        
        [Required] public DateTime DeleteDateTime { get; set; }

        [ForeignKey("PlaylistID")] public virtual IList<PlaylistSong> PlaylistSongs { get; set; }

    }
}
