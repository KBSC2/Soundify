using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class RoleController : DbController<Role>
    {
        public RoleController(IDatabaseContext context) : base(context, context.Roles)
        {
        }
    }
}
