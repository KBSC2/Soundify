using System.ComponentModel.DataAnnotations;

namespace Model.DbModels
{
    public class User : DbModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
<<<<<<< HEAD

        public int Coins { get; set; } = 0;
=======
>>>>>>> PROJ-3-User-Roles
    }
}
