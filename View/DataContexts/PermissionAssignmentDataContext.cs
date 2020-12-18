using Controller.DbControllers;
using Model.Annotations;
using Model.Database.Contexts;
using Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace View.DataContexts
{
    public class PermissionAssignmentDataContext : INotifyPropertyChanged
    {
        public List<string> Roles => RoleController.Create(DatabaseContext.Instance).GetList().Select(x => x.Designation).ToList();
        public List<Permission> Permissions => PermissionController.Create(DatabaseContext.Instance).GetList();
        public Dictionary<Permission, bool[]> PermissionsDict { get; set; }

        private static PermissionAssignmentDataContext instance;
        public static PermissionAssignmentDataContext Instance => instance ??= new PermissionAssignmentDataContext();
    
        public PermissionAssignmentDataContext()
        {
            PermissionsDict = new Dictionary<Permission, bool[]>();
            foreach (var rolePermission in RolePermissionsController.Create(DatabaseContext.Instance)
                .GetPermissionsFromRoles(RoleController.Create(DatabaseContext.Instance).GetList()))
                {
                if (PermissionsDict.ContainsKey(rolePermission.Permission))
                {
                    PermissionsDict[rolePermission.Permission][rolePermission.RoleID - 1] = true;
                }
                else
                {
                    PermissionsDict.Add(rolePermission.Permission, new bool[3] { false, false, false });
                    PermissionsDict[rolePermission.Permission][rolePermission.RoleID - 1] = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}