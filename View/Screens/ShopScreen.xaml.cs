using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for TempAdminScreen.xaml
    /// </summary>
    public partial class ShopScreen : ResourceDictionary
    {
        public ShopScreen()
        {
            InitializeComponent();
        }

        private void ShopItemBuy_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            var shopItem = ShopDataContext.Instance.ShopItems.FirstOrDefault(x => x.ID == id);

            if (shopItem == null || shopItem.Bought)
                return;

            ShopItemController.Create(new DatabaseContext()).BuyItem(UserController.CurrentUser, shopItem);
            ShopDataContext.Instance.OnPropertyChanged();
        }
    }
}