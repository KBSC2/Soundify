using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_RoleDesignation
    {
        private UserController Controller { get; } = new UserController(new MockDatabaseContext());
        private User User { get; set; }

        [SetUp]
        public void SetUp()
        {
            User = new User() { Email = "test@gmail.com", Username = "test" };
            Controller.CreateAccount(User, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [Test]
        public void UserController_UserRoleIDShouldBeUser()
        {
            var result = Controller.GetItem(User.ID);
            Assert.AreEqual(result.RoleID, 2);
        }

        [Test]
        public void UserController_MakeArtiest_UserRoleIDShouldBeArtist()
        {
            Controller.MakeArtist(User);
            var result = User.RoleID;
            Assert.AreEqual(result, 3);
        }

        [TearDown]
        public void TearDown()
        {
            Controller.DeleteItem(User.ID);
        }
    }
}