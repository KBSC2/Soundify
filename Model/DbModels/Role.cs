using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class Role : DbModel
    {
        [Required] public string Designation { get; set; }

        [Required] public string ColorCode { get; set; }

        [ForeignKey("RoleID")] public virtual ICollection<RolePermissions> Permissions { get; set; }
    }
}
