using NUnit.Framework;
using System;
using System.IO;

namespace Tests.Local
{
    [TestFixture]
    public class Controller_FileCacheShould
    {
        private string Path { get; set; }

        [SetUp]
        public void SetUp()
        {
            Path = Controller.FileCache.Instance.GetFile("songs/loveyoubetter.mp3");
        }

        [Test, Category("Local")]
        public void FileCache_GetFile_Path_Exists()
        {
            Assert.IsTrue(File.Exists(Path));
        }

        [Test, Category("Local")]
        public void FileCache_ClearCache_Path_NotExists()
        {
            Controller.FileCache.Instance.ClearCache(TimeSpan.FromSeconds(-1));
            Assert.IsFalse(File.Exists(Path));
        }
    }
}
