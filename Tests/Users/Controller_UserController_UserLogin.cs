using Controller;
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
        private UserController Controller { get; } = new UserController(new MockDatabaseContext());
        private User User { get; set; }

        [SetUp]
        public void SetUp()
        {
            User = new User() { Email = "test@gmail.com", Username = "test" };
            Controller.CreateAccount(User, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [TestCase("test@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]        // Succesfull acocunt login
        [TestCase("no@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.EmailNotFound)]    // Email not found
        [TestCase("test@gmail.com", "oopsie", ExpectedResult = LoginResults.PasswordIncorrect)]         // Succesfull acocunt login
        [TestCase("test", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]                  // Account from username
        public LoginResults UserController_SignupResults(string emailOrUsername, string password)
        {
            var result = Controller.UserLogin(emailOrUsername, password);
            return result;
        }

        [TearDown]
        public void TearDown()
        {
            Controller.DeleteItem(User.ID);
        }
    }
}
