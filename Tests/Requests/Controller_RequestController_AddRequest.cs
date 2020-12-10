using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Requests
{
    [TestFixture]
    public class Controller_RequestController_AddRequest
    {
        private RequestController requestController;

        [SetUp]
        public void SetUp()
        {
            requestController = RequestController.Create(new MockDatabaseContext());

            requestController.CreateItem(new Request()
                { ID = 1, UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing" });
        }

        [Test]
        public void AddRequest()
        {

            requestController.GetItem(1).ContinueWith(res =>
                    Assert.AreEqual(res.Result.ID, 1)
            );
        }
    }
}
