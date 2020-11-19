using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
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

        public User GetUserFromEmail(string email)
        {
            return Set.FirstOrDefault(x => x.Email == email);
        }

        public RegistrationResults CreateAccount(User user, string password, string passwordRepeat)
        {
            if (GetUserFromEmail(user.Email) != null)
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
