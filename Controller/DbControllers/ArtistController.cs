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

        public Artist GetArtistFromUserId(int? userId)
        {
            return GetList().FirstOrDefault(a => a.UserID == userId);
        }

        // Can't this be a little bit more generic. Like update role or something??
        public void MakeArtist(Request request)
        {
            var user = userController.GetItem(request.UserID);

            user.RoleID = 2;
            userController.UpdateItem(user);
            CreateItem(new Artist { ArtistName = request.ArtistName, UserID = user.ID});
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
