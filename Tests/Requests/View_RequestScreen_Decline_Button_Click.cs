using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Requests
{
    [TestFixture]
    public class View_RequestScreen_Decline_Button_Click
    {
        private MockDatabaseContext context;
        private RequestController requestController;
        private Request testRequest;

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            requestController = RequestController.Create(context);
            testRequest = new Request()
                { ID = 1, UserID = 1, User = UserController.Create(context).GetItem(1), ArtistName = "TestArtist", ArtistReason = "Just because I'm testing" };
            requestController.CreateItem(testRequest);
        }

        [Test]
        public void DeclineUserAsArtist()
        {
            requestController.DeclineUser(testRequest);

            Assert.False(requestController.GetList().Contains(testRequest));
            Assert.False(UserController.Create(context).GetItem(testRequest.UserID).RequestedArtist);
        }

        [TearDown]
        public void TearDown()
        {
            requestController.DeleteItem(testRequest.ID);
        }
    }
}