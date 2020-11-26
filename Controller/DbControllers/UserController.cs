using System.Linq;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class UserController : DbController<User>
    {
        public UserController(IDatabaseContext context) : base(context, context.Users)
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
            return GetList().FirstOrDefault(x => x.Email == emailOrUsername || x.Username == emailOrUsername);
        }

        /**
         * Attept to login the user
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
        public RegistrationResults CreateAccount(User user, string password, string passwordRepeat)
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

        public void MakeArtist(User user)
        {
            user.RoleID = 3;
            UpdateItem(user);
        }
    }
}
