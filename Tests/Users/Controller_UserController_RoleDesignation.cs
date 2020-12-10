using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_RoleDesignation
    {
        private UserController userController;
        private ArtistController controller;
        private User user;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            controller = ArtistController.Create(mock);
            userController = UserController.Create(mock);

            user = new User() { ID = 10, Email = "test@gmail.com", Username = "test" };
            userController.CreateAccount(user, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [Test]
        public void UserController_CreateAccount_UserRoleIDShouldBeUser()
        {
            userController.GetItem(user.ID).ContinueWith(res =>
                    Assert.AreEqual(res.Result.RoleID, 1)
            );
        }

        [Test]
        public void UserController_MakeArtist_UserRoleIDShouldBeArtist()
        {
            controller.MakeArtist(user.ID);
            Assert.AreEqual(user.RoleID, 2);
        }

        [Test]
        public void UserController_RevokeArtiest_UserRoleIDShouldBeUser()
        {
            controller.RevokeArtist(user);
            var result = user.RoleID;
            Assert.AreEqual(result, 1);
        }
    }
}