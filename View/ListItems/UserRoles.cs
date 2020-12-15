using Model.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace View.ListItems
{
    public class UserRoles
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public int RoleID { get; set; }
        public UserRoles(User user)
        {
            UserID = user.ID;
            Username = user.Username;
            RoleID = user.RoleID;
            RoleID -= 1;
        }
    }
}
