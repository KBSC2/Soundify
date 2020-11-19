using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class User : DbModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
