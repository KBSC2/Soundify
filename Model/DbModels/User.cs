using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DbModels
{
    public class User : DbModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required] 
        public bool IsActive { get; set; } = false;
        [Required] 
        public bool RequestedArtist { get; set; } = false;
        public string Password { get; set; }
        public int RoleID { get; set; }

        public int Coins { get; set; } = 0;
        public string Token { get; set; }

        public IList<Request> RequestsList { get; set; }
        public IList<UserShopItems> UserShopItems { get; set; }
    }
}
