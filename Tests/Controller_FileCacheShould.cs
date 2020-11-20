using Controller;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace Tests
{
    [TestFixture]
    public class Controller_FileCacheShould
    {
        private string Path { get; set; }

        [SetUp]
        public void SetUp()
        {
            Path = FileCache.Instance.GetFile("songs/untrago.mp3");
        }

        [Test]
        public void GetFile_Path_Exists()
        {
            Assert.IsTrue(File.Exists(Path));
        }

        [Test]
        public void ClearCache_Path_NotExists()
        {
            FileCache.Instance.ClearCache(TimeSpan.FromSeconds(-1));
            Assert.IsFalse(File.Exists(Path));
        }
    }
}
