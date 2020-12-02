using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.DbModels
{
    public class Artist : DbModel
    {
        [Required]
        public int UserID { get; set; }
        public string ArtistName { get; set; }
    }
}
