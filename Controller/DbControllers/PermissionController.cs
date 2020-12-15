using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.EventArgs;
using System;

namespace Controller.DbControllers
{
    public class PermissionController : DbController<Permission>
    {
        #region Events
        public static EventHandler<NoRightsEventArgs> NoRightsEvent;
        #endregion

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns PermissionController : The proxy with a instance of this controller included
         */
        public static PermissionController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PermissionController>(new object[] { context }, context);
        }

        protected PermissionController(IDatabaseContext context) : base(context, context.Permissions)
        {
        }
    }
}
