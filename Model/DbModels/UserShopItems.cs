namespace Model.DbModels
{
    public class UserShopItems : DbModel
    {

        public int UserID { get; set; }

        public virtual User User { get; set; }

        public int ShopItemID { get; set; }

        public virtual ShopItem ShopItem { get; set; }
    }
}
