using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using Soundify;
using View.DataContexts;

namespace View.Screens
{
    /// <summary>
    /// Interaction logic for ShopScreen.xaml
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

            if (shopItem == null || (shopItem.Bought && !shopItem.Repurchasable))
                return;

            ShopItemController.Create(DatabaseContext.Instance).BuyItem(UserController.CurrentUser, shopItem);
            ShopDataContext.Instance.OnPropertyChanged();
            MainWindow.InstanceMainWindow.UpdateButtons();
        }
    }
}