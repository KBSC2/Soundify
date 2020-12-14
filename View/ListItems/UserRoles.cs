using System;
using System.Collections.Generic;
using System.Text;

namespace View.ListItems
{
    public class UserRoles
    {
        public string Username { get; set; }
        public string RoleID { get; set; }
        public UserRoles(string username, int roleID)
        {
            Username = username;
            RoleID = roleID == 1 ? "User" : roleID == 2 ? "Artist" : "Admin" ;
        }
    }
}
