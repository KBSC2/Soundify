using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class Artist : DbModel
    {
        [Required]
        [ForeignKey("Users")]
        public int UserID { get; set; }

        public virtual User User { get; set; }
        public string ArtistName { get; set; }
    }
}
