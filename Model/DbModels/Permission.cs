using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class Permission : DbModel
    {
        [Required]
        public string Name { get; set; }
        public bool HasValue { get; set; } = false;
        
        [NotMapped]
        public int Value { get; set; }
    }
}
