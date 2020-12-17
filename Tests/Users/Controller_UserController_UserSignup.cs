using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using NUnit.Framework;
using static Controller.PasswordController;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_UserController_UserSignup
    {
        private UserController controller;

        [SetUp]
        public void SetUp()
        {
            controller = UserController.Create(new MockDatabaseContext());
            controller.CreateAccount(new User() { ID = 10, Email = "duplicate@gmail.com", Username = "test1"}, "Sterk_W@chtw000rd2",
                "Sterk_W@chtw000rd2"); // create account to test already exists
        }

        [TestCase("", ExpectedResult = PasswordScore.Blank)]                            // empty
        [TestCase("h", ExpectedResult = PasswordScore.VeryWeak)]                        // too short,       no uppercase,   no numbers,     no characters
        [TestCase("hallo", ExpectedResult = PasswordScore.VeryWeak)]                    // too short,       no uppercase,   no numbers,     no characters
        [TestCase("wachtwoord", ExpectedResult = PasswordScore.Weak)]                   // medium length,   no uppercase,   no numbers,     no characters
        [TestCase("HalloLangWachtwoord", ExpectedResult = PasswordScore.Medium)]        // correct length,  with uppercase, no numbers,     no characters
        [TestCase("Wachtw00rdMetC1jfers", ExpectedResult = PasswordScore.Strong)]       // correct length,  with uppercase, with numbers,   no characters
        [TestCase("W@chtw00rdMetAll4s", ExpectedResult = PasswordScore.VeryStrong)]     // correct length,  with uppercase, with numbers,   with characters
        public PasswordScore Controller_PasswordController_TestPasswordScore(string password)
        {
            return CheckStrength(password);
        }

        [TestCase("Hai", "zwak", "zwak", "test2@gmail.com", ExpectedResult = RegistrationResults.PasswordNotStrongEnough)]                      // password not strong enough
        [TestCase("Paul", "match", "no match", "test2@gmail.com", ExpectedResult = RegistrationResults.PasswordNoMatch)]                        // password mismatch
        [TestCase("Sietse", "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "test@gmail.com", ExpectedResult = RegistrationResults.Succeeded)]        // success
        [TestCase("Vincent", "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "duplicate@gmail.com", ExpectedResult = RegistrationResults.EmailTaken)] // email taken
        [TestCase("Ben", "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "SlechtEmail", ExpectedResult = RegistrationResults.InvalidEmail)]           // invalid email
        [TestCase("test1", "Sterk_W@chtw00rd2", "Sterk_W@chtw00rd2", "uniek@gmail.com", ExpectedResult = RegistrationResults.UsernameTaken)]    // username taken
        public RegistrationResults UserController_SignupResults(string username, string password, string repeatPassword, string email)
        {
            return controller.CreateAccount(new User() { ID = 11, Email = email, Username = username }, password, repeatPassword);
        }

        [TestCase("testindb@gmail.com", "SterkWachtw00rd@", ExpectedResult = true)]
        public bool UserController_AccountInDatabase(string email, string password)
        {
            var result = controller.CreateAccount(new User() { ID = 12, Email = email, Username = "test2" }, password, password);
            if (result != RegistrationResults.Succeeded)
                return false;
            return controller.GetUserFromEmailOrUsername(email) != null;
        }

        [TearDown]
        public void TearDown()
        {
            // Remove all accounts from the database, if they exist
            foreach (var i in new [] {10, 11, 12})
                controller.DeleteItem(i);
        }
    }
}
