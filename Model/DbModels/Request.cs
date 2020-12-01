using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class Request : DbModel
    {
        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Songs")]
        public int SongID { get; set; }
        [Required]
        public string RequestType { get; set; }
        public string ArtistName { get; set; }
        public string ArtistReason { get; set; }
    }
}
