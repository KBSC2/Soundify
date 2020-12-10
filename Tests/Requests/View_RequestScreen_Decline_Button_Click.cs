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
        }

        [Test]
        public void DeclineUserAsArtist()
        {
            requestController.CreateItem(testRequest);

            var requestScreen = new RequestScreen();
            requestScreen.DeclineUser(testRequest.ID, context);

            requestController.GetList().ContinueWith(res =>
                Assert.IsFalse(res.Result.Contains(testRequest))
            );

            UserController.Create(context).GetItem(testRequest.UserID).ContinueWith(res =>
                Assert.IsFalse(res.Result.RequestedArtist)
            );
        }
    }
}