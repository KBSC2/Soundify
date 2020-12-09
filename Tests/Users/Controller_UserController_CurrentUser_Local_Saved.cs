using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_CurrentUser_Local_Saved
    {
        private UserController controller;
        private User user;

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
            user = new User() {ID = 10, Email = "test@gmail.com", Username = "test", IsActive = true};
            controller.CreateAccount(user, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
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
            Assert.AreEqual(UserController.CurrentUser, user);
        }
    }
}