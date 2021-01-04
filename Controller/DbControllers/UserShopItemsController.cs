using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class UserShopItemsController
    {
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
            return ProxyController.AddToProxy<UserShopItemsController>(new object[] {context}, context);
        }

        protected UserShopItemsController(IDatabaseContext context)
        {
            this.Context = context;
            Set = Context.UserShopItems;
        }

        /**
         * Get all usershopitems from the tabel
         *
         * @return List<UserShopItems> : All the UserShopItems from the table
         */
        public List<UserShopItems> GetList()
        {
            return Set.ToList();
        }

        /**
         * Insert an item into the table
         *
         * @param item The item to insert
         *
         * @return UserShopItems The created item
         */
        public UserShopItems CreateItem(UserShopItems item)
        {
            item.User = null;
            item.ShopItem = null;

            Set.Add(item);

            if (RealDatabase())
            {
                Context.Entry(item).State = EntityState.Added;
                Context.SaveChanges();
            }

            return item;
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