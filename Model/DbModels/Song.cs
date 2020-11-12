using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class Song : DbModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public double Duration { get; set; }

        public IList<PlaylistSong> PlaylistSongs { get; set; }
    }
}
