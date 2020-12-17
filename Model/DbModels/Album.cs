using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class Album : DbModel
    {
        public string AlbumName { get; set; }

        public int ArtistID { get; set; } // rename column from Artist to ArtistId

        public virtual Artist Artist { get; set; }
        
        public string Description { get; set; }

        [ForeignKey("AlbumID")] public virtual ICollection<Song> Songs { get; set; }
    }
}
