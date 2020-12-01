using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class RequestController : DbController<Request>
    {
        public RequestController(IDatabaseContext context) : base(context, context.Requests)
        {
        }
    }
}
