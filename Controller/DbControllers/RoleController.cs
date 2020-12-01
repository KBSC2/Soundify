using System;
using System.Collections.Generic;
using System.Text;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class RoleController : DbController<Role>
    {
        public static RoleController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RoleController>(new object[] { context }, context);
        }

        protected RoleController(IDatabaseContext context) : base(context, context.Roles)
        {
        }
    }
}
