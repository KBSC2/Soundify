using System.Collections.Generic;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class RequestController : DbController<Request>
    {
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
            return ProxyController.AddToProxy<RequestController>(new object[] {context}, context);
        }

        protected RequestController(IDatabaseContext context) : base(context, context.Requests)
        {
        }

        /**
         * Gets a lists of all the requests to become an artist
         *
         * @return List<Request> A list with all the requests
         */
        public List<Request> GetArtistRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Artist);
        }

        /**
         * Gets a lists of all the requests to upload a song
         *
         * @return List<Request> A list with all the requests
         */
        public List<Request> GetSongRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Song);
        }

        /**
         * Check if artistName and/or artistReason are empty
         * 
         * @param artistName User's requested artistName
         * @param artistReason User's requested artistReason
         * 
         * @return RequestArtistResults : Result of the user's request to become an artist
         */
        public RequestArtistResults RequestArtist(string artistName, string artistReason)
        {
            if (artistName == "" && artistReason == "")
                return RequestArtistResults.NameAndReasonNotFound;
            if (artistName == "")
                return RequestArtistResults.ArtistNameNotFound;
            if (artistReason == "")
                return RequestArtistResults.ReasonNotFound;
            return RequestArtistResults.Success;
        }

        /**
         * Approves a request from a user to become an artist
         *
         * @param requestID The ID of the request in question
         */
        public void ApproveUser(Request request)
        {
            ArtistController.Create(Context).MakeArtist(request);
            DeleteItem(request.ID);
        }

        /**
         * Declines the request from a user to become an artist
         *
         * @param requestID The ID of the request in question
         */
        public void DeclineUser(Request request)
        {
            request.User.RequestedArtist = false;
            UserController.Create(Context).UpdateItem(request.User);

            DeleteItem(request.ID);
        }

        /**
        * Approves a request from a user to upload a song
        *
        * @param requestID The ID of the request in question
        */
        public void ApproveSong(Request request)
        {
            if (request.Song == null) return;
            
            request.Song.Status = SongStatus.Approved;
            SongController.Create(DatabaseContext.Instance).UpdateItem(request.Song);

            DeleteItem(request.ID);
        }

        /**
         * Declines the request from a user to upload a song
         *
         * @param requestID The ID of the request in question
         */
        public void DeclineSong(Request request)
        {
            if (request.Song == null) return;

            DeleteItem(request.ID);

            SongController.Create(DatabaseContext.Instance).DeleteItem(request.Song.ID);
        }
    }
}