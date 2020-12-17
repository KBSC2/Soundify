using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            ShopItems = ShopItemController.Create(DatabaseContext.Instance).GetList(UserController.CurrentUser.ID);
            DataContext.Instance.Timer.Elapsed += OnTimedEvent;
        }
        
        public void OnTimedEvent(object sender, EventArgs e)
        {
            var items = new List<ShopItem>(ShopItems);
            items.ForEach(x =>
            {
                x.Purchasable = UserController.CurrentUser.Coins >= x.Price;
            });
            ShopItems = items;

            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
