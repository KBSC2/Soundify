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
         * This function creates a instance of this controller
         * It adds the controller to the proxy
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

        public int? GetArtistIdFromUserId(int userId)
        {
            return GetList().FirstOrDefault(a => a.UserID == userId)?.ID;
        }

        // Can't this be a little bit more generic. Like update role or something??
        public void MakeArtist(int userId)
        {
            var user = userController.GetItem(userId);

            user.RoleID = 2;
            userController.UpdateItem(user);
            CreateItem(new Artist() { ArtistName = user.Username, UserID = user.ID}); // change user.Username to artist name
        }

        public void RevokeArtist(User user)
        {
            user.RoleID = 1;
            userController.UpdateItem(user);

            var artistId = GetArtistIdFromUserId(user.ID);
            if (artistId != null) DeleteItem((int)artistId);
        }
    }
}
