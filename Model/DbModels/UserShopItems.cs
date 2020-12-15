namespace Model.DbModels
{
    public class UserShopItems : DbModel
    {

        public int UserID { get; set; }

        public User User { get; set; }

        public int ShopItemID { get; set; }

        public ShopItem ShopItem { get; set; }
    }
}
