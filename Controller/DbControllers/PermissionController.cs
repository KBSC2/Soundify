using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class PermissionController : DbController<Permission>
    {
        public static PermissionController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<PermissionController>(new object[] { context });
        }

        protected PermissionController(IDatabaseContext context) : base(context, context.Permissions)
        {
        }
    }
}
