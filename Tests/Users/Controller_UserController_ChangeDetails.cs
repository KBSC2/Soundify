using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_ChangeDetails
    {
        private UserController controller;

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
            controller.CreateAccount(new User() {ID = 11, Email = "testemail@gmail.com", Username = "testUsername"},
                "blabla@!@#123", "blabla@!@#123");
            controller.CreateAccount(new User() { ID = 12, Email = "hai@gmail.com", Username = "Hai" },
                "randomWachtwoord1", "randomWachtwoord1");
        }

        [TestCase("hai@gmail.com", "NewName", ExpectedResult = NewUserInfo.EmailTaken)]                                     // Email taken
        [TestCase("email", "NewName", ExpectedResult = NewUserInfo.InvalidEmail)]                                           // Invalid email
        [TestCase("another_email@gmail.com", "Hai", ExpectedResult = NewUserInfo.UsernameTakenEmailUpdated)]                // Valid email to change, but username taken
        [TestCase("", "Hai", ExpectedResult = NewUserInfo.UsernameTaken)]                                                   // No email changes, username taken
        [TestCase("", "", ExpectedResult = NewUserInfo.Empty)]                                                              // No input
        [TestCase("another_email@gmail.com", "Haiii", ExpectedResult = NewUserInfo.Valid)]                                  // Valid (not taken) email and different username
        [TestCase("another_email@gmail.com", "", ExpectedResult = NewUserInfo.Valid)]                                       // Valid (not taken) email
        [TestCase("", "Haiii", ExpectedResult = NewUserInfo.Valid)]                                                         // Different username
        public NewUserInfo UserController_ChangeDetailsResults(string newEmail, string newUsername)
        {
            return controller.ChangeDetails(new User() { ID = 11, Email = "testemail@gmail.com", Username = "testUsername" }, newEmail, newUsername);
        }
    }
}
