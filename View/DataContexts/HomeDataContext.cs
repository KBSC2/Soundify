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


        public List<ShopItem> AlreadyBought => UserController.CurrentUser == null
            ? null
            : ShopItemController.Create(DatabaseContext.Instance).GetList(UserController.CurrentUser)
                .Where(x => x.Bought && !x.Repurchasable).ToList();


        public List<ShopItem> StillAvailable => UserController.CurrentUser == null
            ? null
            : ShopItemController.Create(DatabaseContext.Instance).GetList(UserController.CurrentUser)
                .Where(x => !x.Bought || x.Repurchasable).ToList();

        

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}