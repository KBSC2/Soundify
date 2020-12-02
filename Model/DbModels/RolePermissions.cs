namespace Model.DbModels
{
    public class RolePermissions
    {
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public int PermissionID { get; set; }
        public Permission Permission { get; set; }

        public int Value { get; set; }
    }
}
