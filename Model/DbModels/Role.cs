using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Model.DbModels
{
    public class Role : DbModel
    {
        [Required] 
        public string Designation { get; set; }
        [Required]
        public string ColorCode { get; set; }
    }
}
