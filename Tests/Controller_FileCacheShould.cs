using Controller;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tests
{
    [TestFixture]
    public class Controller_FileCacheShould
    {
        [SetUp]
        public void SetUp()
        {
            new FileCache();
        }

        [Test]
        public void GetFile_Path_Exists()
        {
            string path = FileCache.Instance.GetFile("test.txt");
            Assert.IsTrue(File.Exists(path));
            FileCache.Instance.ClearCache(TimeSpan.FromSeconds(-1));
        }

        [Test]
        public void ClearCache_Path_NotExists()
        {
            string path = FileCache.Instance.GetFile("test.txt");
            FileCache.Instance.ClearCache(TimeSpan.FromSeconds(-1));
            Assert.IsFalse(File.Exists(path));
        }
    }
}
