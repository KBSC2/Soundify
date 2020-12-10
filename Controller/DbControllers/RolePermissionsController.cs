using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class RolePermissionsController
    {
        private IDatabaseContext Context { get; set; }
        private DbSet<RolePermissions> Set { get; set; }
        
        private RoleController RoleController { get; set; }
        private PermissionController PermissionController { get; set; }

        public static RolePermissionsController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RolePermissionsController>(new object[] { context }, context);
        }


        protected RolePermissionsController(IDatabaseContext context)
        {
            this.Context = context;
            Set = Context.RolePermissions;

            RoleController = RoleController.Create(Context);
            PermissionController = PermissionController.Create(Context);
        }

        public virtual List<RolePermissions> GetList()
        {
            return Set.ToList();
        }

        /**
         * Get all permissions assigned to the role id's
         *
         * @param roleIDs list of role id's
         *
         * Return a list of all permissions
         */
        public List<RolePermissions> GetPermissionsFromRoles(int[] roleIDs)
        {
            var result = GetList().Where(x => roleIDs.Contains(x.RoleID)).ToList();
            result.ForEach(x =>
            {
                x.Permission = PermissionController.GetItem(x.PermissionID).Result;
                x.Role = RoleController.GetItem(x.RoleID).Result;
            });
            return result;
        }

        /**
         * Get the role permissions for a user, by finding a permission from the enum
         *
         * @param user The user to get the permission for
         * @param permission The permission to fetch
         *
         * @return RolePermissions Get a rolePermission object, for the user
         */
        public async Task<RolePermissions> GetPermission(User user, Permissions permission)
        {
            if (user == null)
                return null;

            return GetPermissionsFromRoles(new[] {1, user.RoleID})
                .FirstOrDefault(x => x.PermissionID == (int) permission);
        }

        /**
         * Get the role permission value for a permission, which has a limit value
         *
         * @param user The user to get the permission for
         * @param permission The permission to fetch
         *
         * @return count of what the max value for the permission should be for the user
         */
        public int GetPermissionValueCount(User user, Permissions permission)
        {
            return GetPermissionsFromRoles(new[] {1, user.RoleID})
                .Where(x => x.PermissionID == (int) permission)
                .Max(x => x.Value);
        }
    }
}
