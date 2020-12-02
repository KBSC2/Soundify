using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.Enums;

namespace View.Components
{
    public class PermissionButton : Button
    {
        private Permissions permission;
        public Permissions Permission
        {
            get => permission;
            set
            {
                permission = value;
                UpdateButton();
            }
        }

        private UserController controller;

        public PermissionButton()
        {
            BorderBrush = null;
            controller = UserController.Create(new DatabaseContext());
        }

        public void UpdateButton()
        {
            this.Opacity = controller.HasPermission(UserController.CurrentUser, this.Permission) ? 1f : 0.5f;
        }

        protected override void OnClick()
        {
            if (controller.HasPermission(UserController.CurrentUser, Permission))
            {
                base.OnClick();
            }
            else
            {
                // open not allowed screen thingy
            } 
                
        }

    }
}
