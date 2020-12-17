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
            Set = Context.RolePermissions;
        }

        /**
         * Get all permissions assigned to the role id's
         *
         * @param roleIDs List of role id's
         *
         * @return List<RolePermission> : A list of all permissions
         */
        public List<RolePermissions> GetPermissionsFromRoles(int[] roleIDs)
        {
            var roleController = RoleController.Create(Context);
            var permissionController = PermissionController.Create(Context);

            var result = Set.Where(x => roleIDs.Contains(x.RoleID)).ToList();
            result.ForEach(x =>
            {
                x.Permission = permissionController.GetItem(x.PermissionID);
                x.Role = roleController.GetItem(x.RoleID);
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
            return this.GetPermissionsFromRoles(new[] { 1, user.RoleID })
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
            return this.GetPermissionsFromRoles(new[] {1, user.RoleID})
                .Where(x => x.PermissionID == (int) permission)
                .Max(x => x.Value);
        }

        /**
        * Adds the permission to a role
        * 
        * @param permissionID The permission to fetch
        * @param roleID The roleID to fetch
        * 
        * @return void
        */
        public void AddPermissionToRole(int permissionID, int roleID)
        {
            var rolePermission = new RolePermissions() { PermissionID = permissionID, RoleID = roleID };
            Set.Add(rolePermission);
            if (!RealDatabase()) return;
            Context.Entry(rolePermission).State = EntityState.Added;
            Context.SaveChanges();

        }

        /**
         * Removes the permission from a role
         * 
         * @param permissionID The permission to fetch
         * @param roleID The roleID to fetch
         * 
         * @return void
         */
        public void RemovePermissionFromRole(int permissionID, int roleID)
        {
            var rolePermission = GetPermission(roleID, permissionID);
            Set.Remove(rolePermission);
            if (!RealDatabase()) return;
            Context.Entry(rolePermission).State = EntityState.Deleted;
            Context.SaveChanges();
            
        }

        /**
         * Get the role permissions for a user, by finding a permission from the enum
         *
         * @param user The user to get the permission for
         * @param permission The permission to fetch
         *
         * @return RolePermissions Get a rolePermission object, for the user
         */
        public RolePermissions GetPermission(int roleID, int permission)
        {
            return this.GetPermissionsFromRoles(new[] { 1, roleID })
                .FirstOrDefault(x => x.PermissionID == permission);
        }

        /**
         * Determine if the database is a real database, or a mock database
         *
         * @return bool : Checks if it is a real database or not
         */
        public bool RealDatabase()
        {
            return Context is DatabaseContext;
        }
    }
}
