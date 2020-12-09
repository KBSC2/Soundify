﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class RequestController : DbController<Request>
    {
        private DbSet<RequestController> set { get; set; }
        public static RequestController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RequestController>(new object[] { context }, context);
        }
        protected RequestController(IDatabaseContext context) : base(context, context.Requests)
        {
        }

        /**
         * gets a lists of all the requests to become an artist
         *
         * @return List<Request> a list with all the requests
         */
        public virtual List<Request> GetArtistRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Artist);
        }
    }
}
