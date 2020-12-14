using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class UserShopItemsController
    {
        private static UserShopItemsController instance;

        private IDatabaseContext Context { get; set; }
        private DbSet<UserShopItems> Set { get; set; }


        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns RolePermissionController : The proxy with a instance of this controller included
         */
        public static UserShopItemsController Create(IDatabaseContext context)
        {
            if (instance == null)
                instance = ProxyController.AddToProxy<UserShopItemsController>(new object[] {context}, context);
            return instance;
        }

        protected UserShopItemsController(IDatabaseContext context)
        {
            this.Context = context;
            Set = Context.UserShopItems;
        }

        public List<UserShopItems> GetList()
        {
            var userController = UserController.Create(Context);
            var shopItemController = ShopItemController.Create(Context);

            var result = Set.ToList();
            result.ForEach(x =>
            {
                x.User = userController.GetItem(x.UserID);
                x.ShopItem = shopItemController.GetItem(x.ShopItemID);
            });
            return result;
        }

        public void CreateItem(UserShopItems item)
        {
            item.User = null;
            item.ShopItem = null;

            Set.Add(item);

            if (!RealDatabase()) return;

            Context.Entry(item).State = EntityState.Added;
            Context.SaveChanges();
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

        /**
        * Gets all the shopItems for a user
        *
        * @param userId the id of the user
        *
        * @return List<UserShopItems> : list of items
        */
        public List<UserShopItems> GetItemsForUser(int userId)
        {
            return GetList().Where(x => x.UserID == userId).ToList();
        }
    }
}
