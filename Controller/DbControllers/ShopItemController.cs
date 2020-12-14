using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class ShopItemController : DbController<ShopItem>
    {
        private static ShopItemController instance;
        private UserShopItemsController userShopItemsController;

        public static ShopItemController Create(IDatabaseContext context)
        {
            if (instance == null)
                instance = ProxyController.AddToProxy<ShopItemController>(new object[] { context }, context);
            return instance;
        }

        protected ShopItemController(IDatabaseContext context) : base(context, context.ShopItems)
        {
            if (userShopItemsController == null)
                userShopItemsController = UserShopItemsController.Create(Context);
        }

        public List<ShopItem> GetList(int userId)
        {
            var userItems = UserShopItemsController.Create(Context).GetItemsForUser(userId).Select(x => x.ShopItemID);
            var items = GetList();
            items.ForEach(x => 
                x.Bought = userItems.Contains(x.ID)    
            );
            return items;
        }

        public void BuyItem(User user, ShopItem shopItem)
        {
            shopItem.Bought = true;
            UpdateItem(shopItem);

            userShopItemsController.CreateItem(new UserShopItems()
            {
                ShopItemID = shopItem.ID,
                ShopItem = shopItem,
                UserID = user.ID,
                User = user
            });
        }
    }
}
