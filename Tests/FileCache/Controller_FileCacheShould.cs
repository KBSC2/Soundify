using NUnit.Framework;
using System;
using System.IO;

namespace Tests.FileCache
{
    [TestFixture]
    public class Controller_FileCacheShould
    {
        private string Path { get; set; }

        [SetUp]
        public void SetUp()
        {
            Path = Controller.FileCache.Instance.GetFile("songs/untrago.mp3");
        }

        [Test]
        public void GetFile_Path_Exists()
        {
            Assert.IsTrue(File.Exists(Path));
        }

        [Test]
        public void ClearCache_Path_NotExists()
        {
            Controller.FileCache.Instance.ClearCache(TimeSpan.FromSeconds(-1));
            Assert.IsFalse(File.Exists(Path));
        }
    }
}
