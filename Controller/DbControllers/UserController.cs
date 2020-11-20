using System.Collections.Generic;
using System.Linq;
using Model.Data;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class UserController : DbController<User>
    {
        public UserController(DatabaseContext context) : base(context, context.Users)
        {
        }

        public List<Playlist> GetPlaylists(User user)
        {
            return Context.Playlists.Where(x => x.UserID == user.ID).ToList();
        }

        public User GetUserFromEmailOrUsername(string emailOrUsername)
        {
            return Set.FirstOrDefault(x => x.Email == emailOrUsername || x.Username == emailOrUsername);
        }

        public LoginResults UserLogin(string emailOrUsername, string password)
        {
            var user = new UserController(new DatabaseContext()).GetUserFromEmailOrUsername(emailOrUsername);
            if (user == null)
                return LoginResults.EmailNotFound;

            string passwordHash = PasswordController.EncryptPassword(password);
            if (user.Password != passwordHash)
                return LoginResults.PasswordIncorrect;

            return LoginResults.Success;
        }

        public RegistrationResults CreateAccount(User user, string password, string passwordRepeat)
        {
            if (GetUserFromEmailOrUsername(user.Email) != null)
                return RegistrationResults.EmailTaken;

            if (!password.Equals(passwordRepeat))
                return RegistrationResults.PasswordNoMatch;

            if (PasswordController.CheckStrength(password) < PasswordController.PasswordScore.Strong)
                return RegistrationResults.PasswordNotStrongEnough;

            user.Password = PasswordController.EncryptPassword(password);
            CreateItem(user);
            return RegistrationResults.Succeeded;
        }
    }
}
