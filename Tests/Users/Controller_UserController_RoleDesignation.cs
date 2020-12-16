﻿using System.Collections.Generic;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_RoleDesignation
    {
        private UserController userController;
        private ArtistController controller;
        private User user;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            controller = ArtistController.Create(mock);
            userController = UserController.Create(mock);

            user = new User() { ID = 10, Email = "test@gmail.com", Username = "test" };
            userController.CreateAccount(user, "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [Test]
        public void UserController_CreateAccount_UserRoleIDShouldBeUser()
        {
            var result = userController.GetItem(user.ID);
            Assert.AreEqual(result.RoleID, 1);
        }

        [Test]
        public void UserController_MakeArtiest_UserRoleIDShouldBeArtist()
        {
            controller.MakeArtist(new Request {UserID = user.ID, ArtistName = "test artiest"});
            var result = user.RoleID;
            Assert.AreEqual(result, 2);
        }

        [Test]
        public void UserController_RevokeArtiest_UserRoleIDShouldBeUser()
        {
            controller.RevokeArtist(user);
            var result = user.RoleID;
            Assert.AreEqual(result, 1);
        }

        [Test]
        public void UserController_ChangeUserRole()
        {
            //Get user
            var result = userController.GetItem(user.ID);
            Assert.AreEqual(result.RoleID, 1);

            //Update user to artist
            userController.UpdateUserRole(user.ID, 2);
            Assert.AreEqual(result.RoleID, 2);

            //Update user to admin
            userController.UpdateUserRole(user.ID, 3);
            Assert.AreEqual(result.RoleID, 3);
        }

        [TearDown]
        public void TearDown()
        {
            controller.DeleteItem(user.ID);
        }
    }
}