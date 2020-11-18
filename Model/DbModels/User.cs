using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class User : DbModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("UserID")]
        public ICollection<Playlist> Playlists { get; set; }
    }
}
