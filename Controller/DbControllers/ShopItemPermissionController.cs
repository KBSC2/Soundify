using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class ShopItemPermissionController
    {
        private static ShopItemPermissionController instance;
        private IDatabaseContext Context { get; set; }
        private DbSet<ShopItemPermissions> Set { get; set; }

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns RolePermissionController : The proxy with a instance of this controller included
         */
        public static ShopItemPermissionController Create(IDatabaseContext context)
        {
            if (instance == null)
                instance = ProxyController.AddToProxy<ShopItemPermissionController>(new object[] {context}, context);
            return instance;
        }

        protected ShopItemPermissionController(IDatabaseContext context)
        {
            this.Context = context;
            Set = Context.ShopItemPersmissions;
        }

        public List<ShopItemPermissions> GetList()
        {
            return Set.ToList();
        }

        /**
         * Get all permissions assigned to the role id's
         *
         * @param roleIDs List of role id's
         *
         * @return List<ShopItemPermissions> : A list of all shopItemPermissions
         */
        public List<ShopItemPermissions> GetPermissionsFromShopItems(List<UserShopItems> shopItems)
        {
            var result = new List<ShopItemPermissions>();

            shopItems.ForEach(x =>
            {
                x.ShopItem.Permissions.ToList().ForEach(y =>
                {
                    if (!result.Contains(y))
                        result.Add(y);
                });
            });

            return result;
        }

        /**
         * Get the permissions for a user, by finding a permission from the enum
         *
         * @param user The user to get the permission for
         * @param permission The permission to fetch
         *
         * @return ShopItemPermissions Get a Permission object, for the user
         */
        public ShopItemPermissions GetPermission(User user, Permissions permission)
        {
            if (user == null)
                return null;

            return this.GetPermissionsFromShopItems(user.UserShopItems?.ToList())
                .FirstOrDefault(x => x.PermissionID == (int)permission);
        }

        /**
         * Get the ShopItem permission value for a permission, which has a limited value
         *
         * @param user The user to get the permission for
         * @param permission The permission to fetch
         *
         * @return Sum of what the value for the permission should be for the user
         */
        public int GetPermissionValueCount(User user, Permissions permission)
        {
            if (user == null)
                return 0;

            var perms = this.GetPermissionsFromShopItems(user.UserShopItems?.ToList())
                .Where(x => x.Permission.Name == permission.ToString()).ToList();

            var count = 0;
            foreach (var t in perms)
                count += perms.FirstOrDefault(x => x.ShopItemID == t.ShopItemID)?.Value ?? 0;

            return count;
        }
    }
}
