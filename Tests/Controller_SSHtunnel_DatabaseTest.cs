using Controller;
using Model.Data;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Controller_SSHtunnel_DatabaseTest
    {
        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
        }

        [Test]
        public void IsConnected()
        {
            Assert.IsTrue(new DatabaseContext().Database.CanConnect());
        }
    }
}
