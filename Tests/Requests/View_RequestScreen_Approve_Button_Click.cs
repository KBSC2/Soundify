﻿using Controller.DbControllers;
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

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            requestController = RequestController.Create(context);
            testRequest = new Request()
                {ID = 1, UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing"};
            requestController.CreateItem(testRequest);
        }

        [Test]
        public void ApproveUserAsArtist()
        {
            requestController.ApproveUser(testRequest.ID);

            Assert.False(requestController.GetList().Contains(testRequest));

            var artistId = ArtistController.Create(context).GetArtistIdFromUserId(testRequest.UserID);

            Assert.True(artistId.HasValue);
        }

        [TearDown]
        public void TearDown()
        {
            requestController.DeleteItem(testRequest.ID);

            var artistId = ArtistController.Create(context).GetArtistIdFromUserId(testRequest.UserID);
            if(artistId != null)
                ArtistController.Create(context).DeleteItem((int)artistId);
        }
    }
}