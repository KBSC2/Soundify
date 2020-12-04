using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using System;
using System.Windows;

namespace Controller.DbControllers
{
    public class PermissionController : DbController<Permission>
    {
        #region Events
        public static EventHandler<NoRightsEventArgs> NoRightsEvent;
        #endregion

        public static PermissionController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PermissionController>(new object[] { context }, context);
        }

        protected PermissionController(IDatabaseContext context) : base(context, context.Permissions)
        {
        }
    }
}
