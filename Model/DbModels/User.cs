using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class User : DbModel
    {
        [Required] public string Username { get; set; }

        [Required] public string Email { get; set; }

        [Required] public bool IsActive { get; set; } = false;

        [Required] public bool RequestedArtist { get; set; } = false;

        public string Password { get; set; }

        public int RoleID { get; set; }
        [ForeignKey("RoleID")] public virtual Role Role { get; set; }

        public int Coins { get; set; } = 0;

        public string Token { get; set; }

        [ForeignKey("UserID")] public virtual ICollection<Playlist> Playlists { get; set; }

        [ForeignKey("UserID")] public virtual ICollection<UserShopItems> UserShopItems { get; set; }

        [ForeignKey("UserID")] public virtual IList<Request> RequestsList { get; set; }
    }
}