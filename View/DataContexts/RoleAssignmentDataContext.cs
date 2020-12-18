using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace View.DataContexts
{
    public class RoleAssignmentDataContext : INotifyPropertyChanged
    {

        public string UpdateStatus { get; set; } = "No changes have been made";
        public List<string> Roles => RoleController.Create(DatabaseContext.Instance).GetList().Select(x => x.Designation).ToList();
        public List<User> Users { get; set; } = UserController.Create(DatabaseContext.Instance).GetFilteredList(x => x.ID != UserController.CurrentUser.ID);

        private static RoleAssignmentDataContext instance;
        public static RoleAssignmentDataContext Instance => instance ??= new RoleAssignmentDataContext();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
