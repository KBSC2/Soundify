using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Enums;

namespace Model.DbModels
{
    public class Song : DbModel
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey("Artist")]
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public double Duration { get; set; }
        
        public string WrittenBy { get; set; }

        public string ProducedBy { get; set; }

        public string Description { get; set; }

        public string PathToImage { get; set; }

        public SongStatus Status { get; set; }

        public IList<PlaylistSong> PlaylistSongs { get; set; }
    }
}
