using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;
using static Controller.PasswordController;

namespace Tests
{
    [TestFixture]
    public class Controller_UserController_UserSignup
    {
        private List<string> AccountsToRemove { get; }= new List<string>() { "test@gmail.com", "duplicate@gmail.com", "testindb@gmail.com" };
        private UserController Controller { get; } = new UserController(new DatabaseContext());

        [SetUp]
        public void SetUp()
        {
            DatabaseContext.TEST_DB = true;
            SSHController.Instance.OpenSSHTunnel();
            Controller.CreateAccount(new User() {Email = "duplicate@gmail.com", Username = "test"}, "Sterk_W@chtw000rd2",
                "Sterk_W@chtw000rd2"); // create account to test already exists
        }

        [TestCase("", ExpectedResult = PasswordScore.Blank)]                            // empty
        [TestCase("hallo", ExpectedResult = PasswordScore.VeryWeak)]                    // too short,       no uppercase,   no numbers,     no characters
        [TestCase("wachtwoord", ExpectedResult = PasswordScore.Weak)]                   // medium length,  no uppercase,   no numbers,     no characters
        [TestCase("HalloLangWachtwoord", ExpectedResult = PasswordScore.Medium)]        // correct length,  with uppercase, no numbers,     no characters
        [TestCase("Wachtw00rdMetC1jfers", ExpectedResult = PasswordScore.Strong)]       // correct length, with uppercase, with numbers,   no characters
        [TestCase("W@chtw00rdMetAll4s", ExpectedResult = PasswordScore.VeryStrong)]     // correct length, with uppercase, with numbers,   no characters
        public PasswordScore PasswordController_TestPasswordScore(string password)
        {
            return PasswordController.CheckStrength(password);
        }

        [TestCase("zwak", "zwak", "test2@gmail.com", ExpectedResult = RegistrationResults.PasswordNotStrongEnough)]                     // password not strong enough
        [TestCase("match", "no match", "test2@gmail.com", ExpectedResult = RegistrationResults.PasswordNoMatch)]                        // password mismatch
        [TestCase("Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "test@gmail.com", ExpectedResult = RegistrationResults.Succeeded)]          // succcess
        [TestCase("Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "duplicate@gmail.com", ExpectedResult = RegistrationResults.EmailTaken)]    // email taken
        public RegistrationResults UserController_SignupResults(string password, string repeatPassword, string email)
        {
            var result = Controller.CreateAccount(new User() {Email = email, Username = "test"}, password, repeatPassword);
            return result;
        }

        [TestCase("testindb@gmail.com", "SterkWachtw00rd@", ExpectedResult = true)]
        public bool UserController_AccountInDatabase(string email, string password)
        {
            var result = Controller.CreateAccount(new User() { Email = email, Username = "test" }, password, password);
            if (result != RegistrationResults.Succeeded)
                return false;
            var user = Controller.GetUserFromEmailOrUsername(email);
            return user != null;
        }

        [TearDown]
        public void TearDown()
        {
            AccountsToRemove.ForEach(email =>
            {
                var user = Controller.GetUserFromEmailOrUsername(email);
                if (user != null)
                    Controller.DeleteItem(user.ID);
            });
        }
    }
}
