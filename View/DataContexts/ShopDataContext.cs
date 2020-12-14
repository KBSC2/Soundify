using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class ShopDataContext : INotifyPropertyChanged
    {
        private static ShopDataContext instance;
        public static ShopDataContext Instance => instance ??= new ShopDataContext();

        public List<ShopItem> ShopItems => ShopItemController.Create(new DatabaseContext()).GetList(UserController.CurrentUser.ID);


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
