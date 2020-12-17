using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using View.ListItems;

namespace View.DataContexts
{
    public class RoleAssignmentDataContext : INotifyPropertyChanged
    {

        public string UpdateStatus { get; set; } = "No changes have been made";
        public List<string> Roles => RoleController.Create(DatabaseContext.Instance).GetList().Select(x => x.Designation).ToList();
        public List<User> Users { get; set; }
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
            Users = UserController.Create(DatabaseContext.Instance).GetList();
            Users.Remove(UserController.CurrentUser);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
