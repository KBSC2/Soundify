using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DbModels
{
    public class Artist : DbModel
    {
        [Required]
        public int UserID { get; set; }
        public string ArtistName { get; set; }
        public List<Song> Songs { get; set; }
    }
}
