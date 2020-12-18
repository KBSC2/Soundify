using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Requests
{
    [TestFixture]
    public class View_RequestScreen_Approve_Button_Click
    {
        private MockDatabaseContext context;
        private RequestController requestController;
        private Request testRequest;
        private ArtistController artistController;

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            artistController = ArtistController.Create(context);
            requestController = RequestController.Create(context);
            testRequest = new Request()
                {ID = 1, UserID = 1, User = UserController.Create(context).GetItem(1), ArtistName = "TestArtist", ArtistReason = "Just because I'm testing"};
            requestController.CreateItem(testRequest);
        }

        [Test]
        public void ApproveUserAsArtist()
        {
            requestController.ApproveUser(testRequest);

            Assert.False(requestController.GetList().Contains(testRequest));

            Assert.IsNotNull(ArtistController.Create(context).GetArtistFromUser(testRequest.User));
        }

        [TearDown]
        public void TearDown()
        {
            requestController.DeleteItem(testRequest.ID);

            var artist = ArtistController.Create(context).GetArtistFromUser(testRequest.User);
            if(artist != null)
                ArtistController.Create(context).DeleteItem(artist.ID);
        }
    }
}