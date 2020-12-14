using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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
