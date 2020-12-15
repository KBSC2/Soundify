using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Enums;

namespace Model.DbModels
{
    public class Request : DbModel
    {
        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("Songs")]
        public int? SongID { get; set; }
        public virtual Song Song { get; set; }
        [Required]
        public RequestType RequestType { get; set; }
        public string ArtistName { get; set; }
        public string ArtistReason { get; set; }
    }
}
