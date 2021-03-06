﻿using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_UserLogin
    {
        private UserController controller;

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
            controller.CreateAccount(new User() { ID = 10, Email = "test@gmail.com", Username = "test", IsActive = true },
                "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
            controller.CreateAccount(new User() { ID = 11, Email = "test2@gmail.com", Username = "test2", IsActive = false },
                "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2");
        }

        [TestCase("test@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]        // Successful account login
        [TestCase("no@gmail.com", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.EmailNotFound)]    // Email not found
        [TestCase("test@gmail.com", "oopsie", ExpectedResult = LoginResults.PasswordIncorrect)]         // Successful account login
        [TestCase("test", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.Success)]                  // Account from username
        [TestCase("test2", "Sterk_W@chtw00rd2", ExpectedResult = LoginResults.UserNotActive)]           // Account from username
        public LoginResults UserController_SignupResults(string emailOrUsername, string password)
        {
            return controller.UserLogin(emailOrUsername, password);
        }

        [Test]
        public void UserController_CurrentUser_LoggedIn()
        {
            var email = "test@gmail.com";
            if (controller.UserLogin(email, "Sterk_W@chtw00rd2") == LoginResults.Success) 
                UserController.CurrentUser = controller.GetUserFromEmailOrUsername(email);
            Assert.AreEqual(UserController.CurrentUser, controller.GetUserFromEmailOrUsername(email));
        }
    }
}
