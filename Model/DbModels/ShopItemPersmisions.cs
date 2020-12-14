using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class ShopItemPermissions : DbModel
    {
        public int ShopItemID { get; set; }
        public ShopItem ShopItem { get; set; }

        public int PermissionID { get; set; }
        public Permission Permission { get; set; }

        public int Value { get; set; }
    }
}
