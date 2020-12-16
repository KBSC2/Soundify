using System.Windows;
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
        public bool HideOnBlocked { get; set; }
        private UserController controller;

        public PermissionButton()
        {
            BorderBrush = null;
            controller = UserController.Create(DatabaseContext.Instance);
            DataContexts.DataContext.PermissionButtons.Add(this);
        }

        public void UpdateButton()
        {
            var allowed = controller.HasPermission(UserController.CurrentUser, this.Permission);
            this.Opacity = allowed ? 1f : 0.5f;
            if (HideOnBlocked)
                this.Visibility = allowed ? Visibility.Visible : Visibility.Hidden;
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
