using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class ShopItemController : DbController<ShopItem>
    {
        public static ShopItemController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<ShopItemController>(new object[] { context }, context);
        }

        protected ShopItemController(IDatabaseContext context) : base(context, context.ShopItems)
        {
        }

        /**
         * Get all shopitems from the user, and set all variables specific for the user
         *
         * @param user The user to get the shopitems from
         *
         * @return List<ShopItem> : All user's shopitems
         */
        public List<ShopItem> GetList(User user)
        {
            var items = GetList();
            items.ForEach(x =>
            {
                x.Bought = user.UserShopItems.Select(y => y.ShopItem).ToArray().Contains(x);
                x.Purchasable = user.Coins >= x.Price;
                x.ImagePath ??= "../Assets/NoImage.png";
            });
            return items;
        }

        /**
         * Buy a shopitem for the user
         *
         * @param user The user to buy the item for
         * @param shopItem The shopitem to buy
         */
        public void BuyItem(User user, ShopItem shopItem)
        {
            if (user.Coins < shopItem.Price) return;

            shopItem.Bought = true;
            UpdateItem(shopItem);

            UserShopItemsController.Create(Context).CreateItem(new UserShopItems()
            {
                ShopItemID = shopItem.ID,
                UserID = user.ID,
            });
            UserController.Create(Context).RemoveCoins(user, shopItem.Price);
        }
    }
}