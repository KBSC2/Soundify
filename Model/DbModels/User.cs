﻿using System.ComponentModel.DataAnnotations;

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
