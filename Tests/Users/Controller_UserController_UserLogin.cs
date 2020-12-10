using System.Threading.Tasks;
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

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
        }

        private async Task<bool> CreateAccounts()
        {
            await Task.WhenAll(new Task[]
            {
                controller.CreateAccount(
                    new User() {ID = 10, Email = "test@gmail.com", Username = "test", IsActive = true},
                    "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2"),
                controller.CreateAccount(
                    new User() {ID = 11, Email = "test2@gmail.com", Username = "test2", IsActive = false},
                    "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2")
            });

            return true;
        }

        [TestCase("test@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]        // Successful account login
        [TestCase("no@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.EmailNotFound)]    // Email not found
        [TestCase("test@gmail.com", "oopsie", ExpectedResult = LoginResults.PasswordIncorrect)]         // Successful account login
        [TestCase("test", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]                  // Account from username
        [TestCase("test2", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.UserNotActive)]           // Account from username
        public LoginResults UserController_SignupResults(string emailOrUsername, string password)
        {
            return CreateAccounts().ContinueWith(_ =>
            {
                return controller.UserLogin(emailOrUsername, password).ContinueWith(res =>
                    res.Result
                ).Result;
            }).Result;
        }
    }
}
