using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class HomeDataContext : INotifyPropertyChanged
    {
        private static HomeDataContext instance;
        public static HomeDataContext Instance => instance ??= new HomeDataContext();

        public List<ShopItem> ShopItems { get; set; }

        public List<ShopItem> AlreadyBought => new List<ShopItem>(ShopItems);

        public HomeDataContext()
        {
            ShopItems = ShopItemController.Create(DatabaseContext.Instance).GetList(UserController.CurrentUser);
        }

        public void UpdateItems()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
