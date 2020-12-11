using System;
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

        public virtual List<Request> GetArtistRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Artist);
        }


        public virtual RequestArtistResults RequestArtist(string ArtistName, string ArtistReason)
        {
            if (ArtistName == "" && ArtistReason == "")
                return RequestArtistResults.NameAndReasonNotFound;
            if (ArtistName == "")
                return RequestArtistResults.ArtistNameNotFound;
            if (ArtistReason == "")
                return RequestArtistResults.ReasonNotFound;
            return RequestArtistResults.Success;
        }
    }
}
