using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DbModels
{
    public class ShopItem : DbModel
    {
        public int Price { get; set; }
        public string Name { get; set; }
        public bool Repurchasable { get; set; }
        public string ImagePath { get; set; }

        [NotMapped] 
        public bool Bought { get; set; }
        [NotMapped]
        public bool Purchasable { get; set; }
    }
}
