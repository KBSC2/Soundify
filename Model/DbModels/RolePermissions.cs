namespace Model.DbModels
{
    public class RolePermissions
    {
        public int RoleID { get; set; }
        public virtual Role Role { get; set; }

        public int PermissionID { get; set; }
        public virtual Permission Permission { get; set; }

        public int Value { get; set; }
    }
}
