using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class RequestController : DbController<Request>
    {
        public static RequestController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RequestController>(new object[] { context }, context);
        }
        protected RequestController(IDatabaseContext context) : base(context, context.Requests)
        {
        }

        public virtual async Task<List<Request>> GetArtistRequests()
        {
            return await GetFilteredList(r => r.RequestType == RequestType.Artist);
        }
    }
}
