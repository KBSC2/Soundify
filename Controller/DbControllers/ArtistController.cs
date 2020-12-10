using System.Collections.Generic;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Artist> GetArtistFromUserId(int userId)
        {
            var list = await GetList();
            return list.FirstOrDefault(a => a.UserID == userId);
        }

        // Can't this be a little bit more generic. Like update role or something??
        public async void MakeArtist(int userId)
        {
            var user = await userController.GetItem(userId);

            user.RoleID = 2;
            userController.UpdateItem(user);
            CreateItem(new Artist { ArtistName = user.Username, UserID = user.ID });
        }

        public async void RevokeArtist(User user)
        {
            user.RoleID = 1;
            userController.UpdateItem(user);

            var artistId = await GetArtistFromUserId(user.ID);
            if (artistId != null) DeleteItem(artistId.ID);
        }

        public async Task<Task<List<Song>>> GetSongsForArtist(int userId)
        {
            var songController = SongController.Create(Context);

            var artist = await GetArtistFromUserId(userId);
            return songController.GetFilteredList(x => x.ArtistID == artist.ID);
        }
    }
}
