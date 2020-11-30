using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Soundify;

namespace View.DataContexts
{
    public class SettingsDataContext
    {
        public Role Role { get; set; } = RoleController.Create(new DatabaseContext()).GetItem(UserController.CurrentUser.RoleID);
    }
}
