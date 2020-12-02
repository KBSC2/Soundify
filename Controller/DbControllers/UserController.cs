using System.Collections.Generic;
using System.Linq;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class UserController : DbController<User>
    {
        public static User CurrentUser;

        public static UserController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<UserController>(new object[] {context}, context);
        }

        protected UserController(IDatabaseContext context) : base(context, context.Users)
        {
        }

        /**
         * Get the first user who has the email or username
         *
         * @param emailOrUsername User's email or username to search by
         *
         * @return User object if found, otherwise null
         */
        public virtual User GetUserFromEmailOrUsername(string emailOrUsername)
        {
            return this.GetList().FirstOrDefault(x => x.Email == emailOrUsername || x.Username == emailOrUsername);
        }

        /**
         * Attept to login the user
         *
         * @param emailOrUsername User's email or username to log in
         * @param password Password to check against database's password hash
         *
         * @return LoginResult : Result of the user login
         */
        public virtual LoginResults UserLogin(string emailOrUsername, string password)
        {
            var user = GetUserFromEmailOrUsername(emailOrUsername);
            if (user == null)
                return LoginResults.EmailNotFound;

            string passwordHash = PasswordController.EncryptPassword(password);
            if (user.Password != passwordHash)
                return LoginResults.PasswordIncorrect;

            if (user.IsActive == false)
                return LoginResults.UserNotActive;

            return LoginResults.Success;
        }

        /**
         * Attempt to create an user account
         * Do a few checks on the user's credentials
         *
         * @param user The user database object to insert
         * @param password Password one to check
         * @param passwordRepeat Password two to check
         *
         * @return RegistrationResult : Result of the user account creation
         */
        public virtual RegistrationResults CreateAccount(User user, string password, string passwordRepeat)
        {
            if (GetUserFromEmailOrUsername(user.Email) != null)
                return RegistrationResults.EmailTaken;

            if (!password.Equals(passwordRepeat))
                return RegistrationResults.PasswordNoMatch;

            if (PasswordController.CheckStrength(password) < PasswordController.PasswordScore.Strong)
                return RegistrationResults.PasswordNotStrongEnough;

            user.Password = PasswordController.EncryptPassword(password);
            user.RoleID = 2;
            // Insert user object into database
            CreateItem(user);
            return RegistrationResults.Succeeded;
        }

        // Can't this be a little bit more generic. Like update role or something??
        public void MakeArtist(User user)
        {
            user.RoleID = 3;
            UpdateItem(user);
        }

        public void RevokeArtist(User user)
        {
            user.RoleID = 2;
            UpdateItem(user);
        }

        /**
         * Check if the user has a permission
         *
         * @param user The user database object to insert
         * @param permission The permission to check
         * @param maxValue The permission to check for the maximum allowed value
         *
         * @return user has permission
         */
        public bool HasPermission(User user, Permissions permission, Permissions maxValue)
        {
            if (!HasPermission(user, permission))
                return false;

            var rpController = RolePermissionsController.Create(Context);
            var max = rpController.GetPermissionValueCount(user, maxValue);

            Dictionary<Permissions, int> maxValues = new Dictionary<Permissions, int>()
            {
                { Permissions.PlaylistLimit, PlaylistController.Create(Context).GetActivePlaylists(user.ID).Count },
                { Permissions.PlaylistSongsLimit, 3} //implement current playlist max songs
            };

            return max > maxValues[maxValue];
        }

        /**
         * Check if the user has a permission
         *
         * @param user The user database object to insert
         * @param permission The permission to check
         *
         * @return user has permission
         */
        public bool HasPermission(User user, Permissions permission)
        {
            var controller = RolePermissionsController.Create(Context);
            return controller.GetPermission(user, permission) != null;
        }
    }
}