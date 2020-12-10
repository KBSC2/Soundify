using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_UserLogin
    {
        private UserController controller;
        private User user1;
        private User user2;

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
            user1 = new User() { ID = 10, Email = "test@gmail.com", Username = "test", IsActive = true };
            user2 = new User() { ID = 11, Email = "test2@gmail.com", Username = "test2", IsActive = false };
            controller.CreateAccount(user1, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
            controller.CreateAccount(user2, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [TestCase("test@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]        // Successful account login
        [TestCase("no@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.EmailNotFound)]    // Email not found
        [TestCase("test@gmail.com", "oopsie", ExpectedResult = LoginResults.PasswordIncorrect)]         // Successful account login
        [TestCase("test", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]                  // Account from username
        [TestCase("test2", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.UserNotActive)]           // Account from username
        public LoginResults UserController_SignupResults(string emailOrUsername, string password)
        {
            var result = controller.UserLogin(emailOrUsername, password);
            return result;
        }
        [Test]
        public void UserController_CurrentUser_LoggedIn()
        {
            var email = "test@gmail.com";
            var password = "Sterk_W@chtw00rd2";
            var result = controller.UserLogin(email, password);
            if (result == LoginResults.Success)
            {
                UserController.CurrentUser = controller.GetUserFromEmailOrUsername(email);
            }
            Assert.AreEqual(UserController.CurrentUser, user1);
        }
    }
}
