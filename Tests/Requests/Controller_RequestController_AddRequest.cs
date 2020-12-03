using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Requests
{
    [TestFixture]
    public class Controller_RequestController_AddRequest
    {
        private MockDatabaseContext _context;
        private RequestController _requestController;
        private Request _testRequest;
        [SetUp]
        public void SetUp()
        {
            _context = new MockDatabaseContext();
            _requestController = RequestController.Create(_context);
            _testRequest = new Request()
                {ID = 1, UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing"};

        }

        [Test]
        public void AddRequest()
        {
            _requestController.CreateItem(_testRequest);

            var requestItem = _requestController.GetItem(_testRequest.ID);

            Assert.AreEqual(requestItem.ArtistName, "TestArtist");
            Assert.AreEqual(requestItem.UserID, 1);
            Assert.AreEqual(requestItem.ArtistReason, "Just because I'm testing");
        }

        [TearDown]
        public void TearDown()
        {
            _requestController.DeleteItem(_testRequest.ID);
        }
    }
}
