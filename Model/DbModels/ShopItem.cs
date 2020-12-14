using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.DbModels
{
    public class ShopItem : DbModel
    {
        public int Price { get; set; }
        public string Name { get; set; }

        [NotMapped] 
        public bool Bought { get; set; }
    }
}
