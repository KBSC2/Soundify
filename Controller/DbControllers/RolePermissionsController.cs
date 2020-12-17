using System.Collections.Generic;
using System.Linq;
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

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns RolePermissionController : The proxy with a instance of this controller included
         */
        public static RolePermissionsController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RolePermissionsController>(new object[] { context }, context);
        }

        protected RolePermissionsController(IDatabaseContext context)
        {
            this.Context = context;
            this.Set = Context.RolePermissions;
        }

        /**
         * Get all RolePermissions from the table
         *
         * @return List<RolePermission> : All RolePermissions from the table
         */
        public List<RolePermissions> GetList()
        {
            return Set.ToList();
        }

        /**
         * Get all permissions assigned to the role id's
         *
         * @param roleIDs List of role id's
         *
         * @return List<RolePermission> : A list of all permissions
         */
        public List<RolePermissions> GetPermissionsFromRoles(List<Role> roles)
        {
            var result = new List<RolePermissions>();

            roles.ForEach(x =>
            {
                x.Permissions.ToList().ForEach(y =>
                {
                    if (!result.Contains(y))
                        result.Add(y);
                });
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
        public RolePermissions GetPermission(User user, Permissions permission)
        {
            if (user == null)
                return null;

            return this.GetPermissionsFromRoles(GetUserRoles(user))
                .FirstOrDefault(x => x.PermissionID == (int)permission);
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
            if (user == null)
                return 0;

            return this.GetPermissionsFromRoles(GetUserRoles(user))
                .Where(x => x.PermissionID == (int) permission)
                .Max(x => x.Value);
        }

        /**
         * Get all the user roles, add role 1 by default
         *
         * @param user The user to get the roles from
         *
         * @return List<Role> List of the user's roles
         */
        private List<Role> GetUserRoles(User user)
        {
            List<Role> roles = new List<Role>();
            if (user.RoleID != 1)
                roles.Add(RoleController.Create(Context).GetItem(1));
            roles.Add(user.Role);
            return roles;
        }
    }
}