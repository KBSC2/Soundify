using System;
using System.Collections.Generic;
using System.Text;
using Controller;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class EmailTest
    {
        EmailController _emailController = new EmailController();

        [Test]
        public void Controller_EmailController_SendSimpleMail()
        {
            _emailController.SendEmail();
        }
    }
}
