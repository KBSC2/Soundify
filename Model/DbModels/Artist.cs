using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class Artist : DbModel
    {
        [Required] public int UserID { get; set; }
        [ForeignKey("UserID")] public virtual User User { get; set; }

        public string ArtistName { get; set; }

        [ForeignKey("ArtistID")] public virtual ICollection<Album> Albums { get; set; }
        [ForeignKey("ArtistID")] public virtual ICollection<Song> Singles { get; set; }

    }
}
