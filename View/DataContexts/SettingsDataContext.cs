using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.DataContexts
{
    public class SettingsDataContext
    {
        public Role Role { get; set; } = RoleController.Create(new DatabaseContext()).GetItem(UserController.CurrentUser.RoleID);
        public bool HasArtistRequested { get; set; } = UserController.Create(new DatabaseContext())
                .GetItem(UserController.CurrentUser.ID).RequestedArtist;

    }
}
