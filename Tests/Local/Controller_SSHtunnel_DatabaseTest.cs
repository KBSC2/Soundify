using Controller;
using Model.Database.Contexts;
using NUnit.Framework;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_SSHTunnel_DatabaseTest
    {
        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
        }

        [Test, Category("Local")]
        public void IsConnected()
        {
            Assert.IsTrue(new DatabaseContext().Database.CanConnect());
        }
    }
}
