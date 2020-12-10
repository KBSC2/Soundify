using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public int Coins { get; set; } = 0;
        public string Token { get; set; }

        public IList<Request> RequestsList { get; set; }
    }
}
