using System.Collections.Generic;
using System.Linq;
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
            var userCoins = UserController.Create(Context).GetItem(userId).Coins;
            var items = GetList();
            items.ForEach(x =>
            {
                x.Bought = userItems.Contains(x.ID);
                x.Purchasable = userCoins >= x.Price;
                x.ImagePath ??= "../Assets/NoImage.png";
            });
            return items;
        }

        public void BuyItem(User user, ShopItem shopItem)
        {
            if (user.Coins < shopItem.Price) return;

            shopItem.Bought = true;
            UpdateItem(shopItem);

            userShopItemsController.CreateItem(new UserShopItems()
            {
                ShopItemID = shopItem.ID,
                ShopItem = shopItem,
                UserID = user.ID,
                User = user
            });
            UserController.Create(Context).RemoveCoins(user, shopItem.Price);
        }
    }
}
