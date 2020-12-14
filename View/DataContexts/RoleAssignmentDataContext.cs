using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using View.ListItems;

namespace View.DataContexts
{
    public class RoleAssignmentDataContext : INotifyPropertyChanged
    {
        public List<UserRoles> UserRoles { get; set; }
        public List<User> Users => UserController.Create(new DatabaseContext()).GetList();
        private static RoleAssignmentDataContext instance;
        public static RoleAssignmentDataContext Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoleAssignmentDataContext();
                return instance;
            }
        }
        public RoleAssignmentDataContext()
        {
            UserRoles = new List<UserRoles>();
            foreach (User u in Users)
            {
                UserRoles.Add(new UserRoles(u.Username, u.RoleID));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
