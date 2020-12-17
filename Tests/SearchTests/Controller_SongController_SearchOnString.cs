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
        private ArtistController artistController;

        [SetUp]
        public void SetUp()
        {
            var mockDatabaseContext = new MockDatabaseContext();

            artistController = ArtistController.Create(mockDatabaseContext);
            artistController.CreateItem(new Artist() { ID = 11, ArtistName = "Tester", UserID = 1 });

            songController = SongController.Create(mockDatabaseContext);
            songController.CreateItem(new Song() { ID = 1, Name = "SongNameTest", ArtistID = 11, Duration = 10, Path = "../path" });
            songController.CreateItem(new Song() { ID = 2, Name = "Ik WIl Testen, liefst ieder kwartier", ArtistID = 11, Duration = 10, Path = "../path" });
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
    }
}
