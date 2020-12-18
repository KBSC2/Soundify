﻿using System;
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

        /*public List<ShopItem> AlreadyBought => UserShopItemsController.Create(DatabaseContext.Instance).GetList()
            .Where(x => x.User == UserController.CurrentUser).FirstOrDefault().ShopItem;*/

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
