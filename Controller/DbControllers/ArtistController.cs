using System.Collections.Generic;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using System.Linq;
using Model.Annotations;

namespace Controller.DbControllers
{
    public class ArtistController : DbController<Artist>
    {
        private UserController userController;

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns ArtistController : the proxy with a instance of this controller included
         */
        public static ArtistController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<ArtistController>(new object[] { context }, context);
        }

        protected ArtistController(IDatabaseContext context) : base(context, context.Artists)
        {
            userController = UserController.Create(Context);
        }

        /**
         * Returns the artistId based on the userId
         *
         * @param userId int of the userID
         *
         * @return int : the artistId
         */
        public int? GetArtistIdFromUserId(int userId)
        {
            return GetList().FirstOrDefault(a => a.UserID == userId)?.ID;
        }

        public Artist GetArtistFromUserId(int? userId)
        {
            return GetList().FirstOrDefault(a => a.UserID == userId);
        }
        
        /**
         * Grants the role of artist to a user
         *
         * @param userId userid of the of the user
         *
         *  @return void
         */
        // Can't this be a little bit more generic. Like update role or something??
        public void MakeArtist(Request request)

        {
            var user = userController.GetItem(request.UserID);

            user.RoleID = 2;
            userController.UpdateItem(user);
            CreateItem(new Artist { ArtistName = request.ArtistName, UserID = user.ID});
        }

        /**
         * Revokes the role of artist back to a user
         *
         * @param user A User data object
         *
         *  @return void
         */
        public void RevokeArtist(User user)
        {
            user.RoleID = 1;
            userController.UpdateItem(user);

            var artistId = GetArtistIdFromUserId(user.ID);
            if (artistId != null) DeleteItem((int)artistId);
        }
    }
}
