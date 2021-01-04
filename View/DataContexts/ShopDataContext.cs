using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class ShopDataContext : INotifyPropertyChanged
    {
        private static ShopDataContext instance;
        public static ShopDataContext Instance => instance ??= new ShopDataContext();

        public List<ShopItem> ShopItems { get; set; }

        public ShopDataContext()
        {
            ShopItems = ShopItemController.Create(DatabaseContext.Instance).GetList(UserController.CurrentUser);
            CoinsController.Instance.UserCoinsEarned += Update;
            Update(null,null);
        }
        
        public void Update(object sender, EventArgs e)
        {
            var items = new List<ShopItem>(ShopItems);
            items.ForEach(x =>
            {
                x.Bought = UserController.CurrentUser.UserShopItems?.Select(y => y.ShopItem).ToArray().Contains(x) ?? false;
                x.Purchasable = UserController.CurrentUser.Coins >= x.Price;
                x.ImagePath = FileCache.Instance.GetFile(x.ImagePath);
            });

            ShopItems = items;
            OnPropertyChanged("ShopItems");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
