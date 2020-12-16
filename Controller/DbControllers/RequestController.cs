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
        public virtual List<Request> GetArtistRequests()
        {
            return GetFilteredList(r => r.RequestType == RequestType.Artist);
        }



        /**
         * Gets a lists of all the requests to upload a song
         *
         * @return List<Request> A list with all the requests
         */
        public virtual List<Request> GetSongRequests()
        {
            var x = GetFilteredList(r => r.RequestType == RequestType.Song);
            return x;
        }


        /**
         * Check if artistName and/or artistReason are empty
         * 
         * @param artistName User's requested artistName
         * @param artistReason User's requested artistReason
         * 
         * @return RequestArtistResults : Result of the user's request to become an artist
         */
        public virtual RequestArtistResults RequestArtist(string artistName, string artistReason)
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


        /**
        * Approves a request from a user to upload a song
        *
        * @param requestID The ID of the request in question
        */
        public void ApproveSong(int requestID)
        {
            var request = GetItem(requestID);

            if (!request.SongID.HasValue) return;

            var songController = SongController.Create(DatabaseContext.Instance);

            var song = songController.GetItem(request.SongID.Value);
            song.Status = SongStatus.Approved;
            songController.UpdateItem(song);

            DeleteItem(requestID);
        }

        /**
         * Declines the request from a user to upload a song
         *
         * @param requestID The ID of the request in question
         */
        public void DeclineSong(int requestID)
        {
            var request = GetItem(requestID);

            if (!request.SongID.HasValue) return;

            var songController = SongController.Create(DatabaseContext.Instance);

            DeleteItem(requestID);

            songController.DeleteItem(request.SongID.Value);
        }

        /**
         * Get the number of all the pending requests
         *
         * @return int The number of all the requests
         */
        public int GetAllRequestsCount()
        {
            return GetList().Count;
        }
    }
}

