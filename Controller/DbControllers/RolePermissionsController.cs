using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<RolePermissions> GetPermissionsFromRoles(int[] roleIDs)
        {
            var result = Set.Where(x => roleIDs.Contains(x.RoleID)).ToList();
            result.ForEach(x =>
            {
                x.Permission = PermissionController.GetItem(x.PermissionID);
                x.Role = RoleController.GetItem(x.RoleID);
            });
            return result;
        }

        public RolePermissions GetPermission(User user, Permissions permission)
        {
            if (user == null)
                return null;
            var perms = this.GetPermissionsFromRoles(new[] { 1, user.RoleID })
                .Where(x => x.PermissionID == (int)permission);
            return perms.FirstOrDefault();
        }

        public int GetPermissionValueCount(User user, Permissions permission)
        {
            return this.GetPermissionsFromRoles(new[] {1, user.RoleID})
                .Where(x => x.PermissionID == (int) permission)
                .Max(x => x.Value);
        }
    }
}
