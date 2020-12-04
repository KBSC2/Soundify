using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class RolePermissions
    {
        [ForeignKey("Roles")]
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionID { get; set; }
        public virtual Permission Permission { get; set; }

        public int Value { get; set; }
    }
}
