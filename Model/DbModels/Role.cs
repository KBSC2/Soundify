using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DbModels
{
    public class Role : DbModel
    {
        [Required] 
        public string Designation { get; set; }
        [Required]
        public string ColorCode { get; set; }

        public List<User> Users { get; set; }
    }
}
