using System;
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
        public static List<Permission> PermissionsCache { get; set; } = new List<Permission>();

        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @returns the proxy with a instance of this controller included
         */
        public static UserController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<UserController>(new object[] { context }, context);
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
        public User GetUserFromEmailOrUsername(string emailOrUsername)
        {
            return this.GetList().FirstOrDefault(x =>
                string.Equals(x.Email, emailOrUsername, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(x.Username, emailOrUsername, StringComparison.CurrentCultureIgnoreCase));
        }

        /**
         * Attempt to login the user
         *
         * @param emailOrUsername User's email or username to log in
         * @param password Password to check against database's password hash
         *
         * @return LoginResult : Result of the user login
         */
        public LoginResults UserLogin(string emailOrUsername, string password)
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
         * Attempt to create a user account
         * Do a few checks on the user's credentials
         *
         * @param user The user database object to insert
         * @param password Password one to check
         * @param passwordRepeat Password two to check
         *
         * @return RegistrationResult : Result of the user account creation
         */
        public RegistrationResults CreateAccount(User user, string password, string passwordRepeat)
        {
            user.Username = user.Username.ToLower();
            user.Email = user.Email.ToLower();

            if (user.Username == "")
                return RegistrationResults.NoName;

            if (user.Email == "")
                return RegistrationResults.NoEmail;

            if (!user.Email.Contains("@"))
                return RegistrationResults.InvalidEmail;

            if (GetUserFromEmailOrUsername(user.Email) != null)
                return RegistrationResults.EmailTaken;

            if (GetUserFromEmailOrUsername(user.Username) != null)
                return RegistrationResults.UsernameTaken;

            if (!password.Equals(passwordRepeat))
                return RegistrationResults.PasswordNoMatch;

            if (PasswordController.CheckStrength(password) < PasswordScore.Strong)
                return RegistrationResults.PasswordNotStrongEnough;

            user.Password = PasswordController.EncryptPassword(password);
            user.RoleID = 1;
            // Insert user object into database
            CreateItem(user);
            return RegistrationResults.Succeeded;
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

            var max = GetPermissionValue(user, maxValue);
            

            var maxValues = new Dictionary<Permissions, int>()
            {
                { Permissions.PlaylistLimit, PlaylistController.Create(Context).GetActivePlaylists(user).Count },
                { Permissions.PlaylistSongsLimit, 3} //TODO: implement current playlist max songs
            };

            return max > maxValues[maxValue];
        }

        /**
         * Changes the details of a user
         *
         * @param user The user database object to insert
         * @param newEmail New given email to check 
         * @param newUsername New given username to check
         *
         * @return NewUserInfo, if it's valid the respective strings have been updated within the function.
         */

        public virtual NewUserInfo ChangeDetails(User user, string newEmail, string newUsername)
        {
            if (newEmail == "" && newUsername == "")
                return NewUserInfo.Empty;
            
            var email = GetUserFromEmailOrUsername(newEmail);
            var username = GetUserFromEmailOrUsername(newUsername);
            var updatedEmail = false;

            if (newEmail != "")
            {
                if (email != null)
                    return NewUserInfo.EmailTaken;
                if (!newEmail.Contains("@"))
                    return NewUserInfo.InvalidEmail;
                user.Email = newEmail;
                updatedEmail = true;
                UpdateItem(user);
            }
            if (newUsername != "")
            {
                if (username != null && updatedEmail == true)
                    return NewUserInfo.UsernameTakenEmailUpdated;
                if (username != null)
                    return NewUserInfo.UsernameTaken;
                user.Username = newUsername;
                UpdateItem(user);
            }
            return NewUserInfo.Valid;
        }

        /**
         * Checks the max value for the permission
         *
         * @param user The user to check the permission for
         * @param maxPermission The permission to check the max value of
         *
         * @return Count of max value
         */
        public int GetPermissionValue(User user, Permissions maxPermission)
        {
            var sipController = ShopItemPermissionController.Create(Context);
            var rpController = RolePermissionsController.Create(Context);

            var max = rpController.GetPermissionValueCount(user, maxPermission) + sipController.GetPermissionValueCount(user, maxPermission);
            return max;
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
            if (user == null)
                return false;

            var cached = PermissionsCache.FirstOrDefault(x => x.Name == permission.ToString());
            if (cached != null)
                return true;

            var sipController = ShopItemPermissionController.Create(Context);
            var rpController = RolePermissionsController.Create(Context);

            var rpPerm = rpController.GetPermission(user, permission);
            var sipPerm = sipController.GetPermission(user, permission);

            if (rpPerm != null || sipPerm != null)
            {
                var perm = rpPerm != null ? rpPerm.Permission : sipPerm.Permission;
                PermissionsCache.Add(perm);
                return true;
            }

            return false;
        }

        /**
         * Adds coins to the currentUser's account
         *
         * @param user CurrentUser
         * @param coins Number of coins that need to be added
         *
         * @return void
         */
        public User AddCoins(User user, int coins = 1)
        {
            user.Coins += coins;
            UpdateItem(user);
            return user;
        }

        /**
          * Removes coins to the currentUser's account
          *
          * @param user CurrentUser
          * @param coins Number of coins that need to be removed
          *
          * @return void
          */
        public void RemoveCoins(User user, int coins = 1)
        {
            user.Coins -= coins;
            UpdateItem(user);
        }

        /**
          * Updates the role of the given user's account to the give role
          *
          * @param userID The ID of the user whichof the role needs to be changed
          * @param roleID The ID of what role the user needs to be
          *
          * @return void
          */
        public void UpdateUserRole(User user, int roleID)
        {
            user.RoleID = roleID;
            UpdateItem(user);
        }
    }
}