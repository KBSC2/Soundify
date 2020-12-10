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
    public class View_RequestScreen_Approve_Button_Click
    {
        private MockDatabaseContext context;
        private RequestController requestController;
        private ArtistController artistController;
        private Request testRequest;

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            requestController = RequestController.Create(context);
            artistController = ArtistController.Create(context);
            testRequest = new Request()
                {ID = 1, UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing"};
        }

        [Test]
        public void ApproveUserAsArtist()
        {
            requestController.CreateItem(testRequest);
                
            var requestScreen = new RequestScreen();
            requestScreen.ApproveUser(testRequest.ID, context);

            requestController.GetList().ContinueWith(res =>
                res.Result.Contains(testRequest)).ContinueWith(res =>
                Assert.IsFalse(res.Result)
            );


            artistController.GetArtistFromUserId(testRequest.UserID).ContinueWith(res =>
            {
                Assert.NotNull(res.Result);
            });
        }
    }
}