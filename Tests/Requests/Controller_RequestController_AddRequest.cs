using System;
using System.Collections.Generic;
using System.Text;
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
            _requestController = new RequestController(_context);
            _testRequest = new Request()
                {UserID = 1, ArtistName = "TestArtist", ArtistReason = "Just because I'm testing"};

        }
    }
}
