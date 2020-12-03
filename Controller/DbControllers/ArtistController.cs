using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using System.Linq;

namespace Controller.DbControllers
{
    public class ArtistController : DbController<Artist>
    {
        public static ArtistController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<ArtistController>(new object[] { context }, context);
        }

        protected ArtistController(IDatabaseContext context) : base(context, context.Artists)
        {
        }

        public int? GetArtistIDFromUserID(int userID)
        {
            return GetList().FirstOrDefault(a => a.UserID == userID)?.ID;
        }
    }
}
