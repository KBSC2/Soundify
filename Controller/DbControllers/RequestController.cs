using System.Collections.Generic;
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

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns RequestController : The proxy with a instance of this controller included
         */
        public static RequestController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<RequestController>(new object[] { context }, context);
        }
        protected RequestController(IDatabaseContext context) : base(context, context.Requests)
        {
        }

        /**
         * Gets a lists of all the requests to become an artist
         *
         * @return List<Request> A list with all the requests
         */
        public virtual List<Request> GetArtistRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Artist);
        }

        /**
         * Approves a request from a user to become an artist
         *
         * @param requestID The ID of the request in question
         */
        public void ApproveUser(int requestID)
        {
            var request = GetItem(requestID);

            ArtistController.Create(Context).MakeArtist(request);

            DeleteItem(requestID);
        }


        /**
         * Declines the request from a user to become an artist
         *
         * @param requestID The ID of the request in question
         */
        public void DeclineUser(int requestID)
        {
            var request = GetItem(requestID);
            var userID = request.UserID;

            var userController = UserController.Create(Context);
            var user = userController.GetItem(userID);
            user.RequestedArtist = false;
            userController.UpdateItem(user);

            DeleteItem(requestID);
        }
    }
}
