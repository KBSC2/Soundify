using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DbModels
{
    public class Playlist : DbModel 
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Genre { get; set; }
        public DateTime CreationDate { get; set; }

        public IList<PlaylistSong> PlaylistSongs { get; set; }
    }
}
