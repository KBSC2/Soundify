using System.Collections.Generic;
using System.Linq;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.SearchTests
{
    [TestFixture]
    public class Controller_Song_SearchOnString
    {
        private SongController songController;

        [SetUp]
        public void SetUp()
        {
            songController = SongController.Create(new MockDatabaseContext());
            songController.CreateItem(new Song() { ID = 1, Name = "SongNameTest", Artist = "Coder", Duration = 10, Path = "../path" });
            songController.CreateItem(new Song() { ID = 2, Name = "Ik WIl Testen, liefst ieder kwartier", Artist = "Tester", Duration = 10, Path = "../path" });
        }

        [TestCase("SongNameTest", 1)]
        [TestCase("meTest", 1)]
        [TestCase("Tester", 2)]
        [TestCase("ste", 2)]
        public void SongController_Search(string search, int expectedId)
        {
            Assert.IsTrue(songController.SearchSongsOnString(new List<string>() { search })
                .Any(x => x.ID == expectedId));
        }

        [TestCase(new[] { "SongNameTest", "Tester" }, new[] { 1, 2 })]
        [TestCase(new[] { "ameTes", "este" }, new[] { 1, 2 })]
        public void PlaylistController_Multiple(string[] searches, int[] expected)
        {
            Assert.IsFalse(songController.SearchSongsOnString(searches.ToList())
                .Any(x => !expected.Contains(x.ID)));
        }


        [TearDown]
        public void TearDown()
        {
            foreach (var i in new[] {1, 2})
                songController.DeleteItem(i);
        }
    }
}
