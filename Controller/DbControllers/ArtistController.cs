using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using System.Linq;

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
         * @returns the proxy with a instance of this controller included
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
         * @return int of the artistId
         */
        public int? GetArtistIdFromUserId(int userId)
        {
            return GetList().FirstOrDefault(a => a.UserID == userId)?.ID;
        }

        /**
         * Grants the role of artist to a user
         *
         * @param userId userid of the of the user
         */
        public void MakeArtist(int userId)
        {
            var user = userController.GetItem(userId);

            user.RoleID = 2;
            userController.UpdateItem(user);
            CreateItem(new Artist() { ArtistName = user.Username, UserID = user.ID}); // change user.Username to artist name
        }

        /**
         * Revokes the role of artist back to a user
         *
         * @param user A User data object
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
