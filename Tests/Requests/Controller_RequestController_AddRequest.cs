﻿using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Requests
{
    [TestFixture]
    public class Controller_RequestController_AddRequest
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

        }

        [Test]
        public void AddRequest()
        {
            requestController.CreateItem(testRequest);

            var requestItem = requestController.GetItem(testRequest.ID);

            Assert.AreEqual(requestItem, testRequest);
        }

        [TearDown]
        public void TearDown()
        {
            requestController.DeleteItem(testRequest.ID);
        }
    }
}
