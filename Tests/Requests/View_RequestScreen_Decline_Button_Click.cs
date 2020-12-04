using System.Windows;
using System.Windows.Controls;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using View.Screens;

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
                { ID = 1, UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing" };
            requestController.CreateItem(testRequest);
        }

        [Test]
        public void DeclineUserAsArtist()
        {
            var requestScreen = new RequestScreen();
            requestScreen.DeclineUser(testRequest.ID, context);

            Assert.False(RequestController.Create(context).GetList().Contains(testRequest));
            Assert.False(UserController.Create(context).GetItem(testRequest.UserID).RequestedArtist);
        }

        [TearDown]
        public void TearDown()
        {
            requestController.DeleteItem(testRequest.ID);
        }
    }
}