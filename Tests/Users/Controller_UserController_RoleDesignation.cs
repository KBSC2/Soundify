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
        private ArtistController artistController;
        private User user;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            artistController = ArtistController.Create(mock);
            userController = UserController.Create(mock);

            user = new User() { ID = 10, Email = "test@gmail.com", Username = "test", RoleID = 1};
            userController.CreateAccount(user, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [Test]
        public void UserController_CreateAccount_UserRoleIDShouldBeUser()
        {
            Assert.AreEqual(userController.GetItem(user.ID).RoleID, 1);
        }

        [Test]
        public void UserController_MakeArtist_UserRoleIDShouldBeArtist()
        {
            artistController.MakeArtist(new Request { UserID = user.ID, User = user, ArtistName = "test artiest" });
            Assert.AreEqual(user.RoleID, 2);
        }

        [Test]
        public void UserController_ChangeUserRole()
        {
            //Get user
            Assert.AreEqual(user.RoleID, 1);

            //Update user to artist
            userController.UpdateUserRole(user, 2);
            Assert.AreEqual(user.RoleID, 2);

            //Update user to admin
            userController.UpdateUserRole(user, 3);
            Assert.AreEqual(user.RoleID, 3);
        }

        [TearDown]
        public void TearDown()
        {
            artistController.DeleteItem(user.ID);
        }
    }
}