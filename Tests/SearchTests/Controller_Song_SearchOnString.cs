using System.Collections.Generic;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.SearchTests
{
    [TestFixture]
    public class Controller_Song_SearchOnString
    {
        private MockDatabaseContext context;
        private SongController songController;
        private Song songNameTest;
        private Song songArtistTest;
        private List<Song> songSearchResults;

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            songController = new SongController(context);
            songNameTest = new Song(){Name = "SongNameTest", Artist = "Coder", Duration = 10, Path = "../path"};
            songArtistTest = new Song() { Name = "Ik WIl Testen, liefst ieder kwartier", Artist = "Tester", Duration = 10, Path = "../path" };
            songController.CreateItem(songNameTest);
            songController.CreateItem(songArtistTest);
            songSearchResults = new List<Song>();
        }

        [Test]
        public void Controller_SongController_SearchOnFullName()
        {
            var confirm = false;
            songSearchResults = songController.SearchSongsOnString(new List<string>() { "SongNameTest" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songNameTest.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void Controller_SongController_SearchOnPartialName()
        {
            var confirm = false;
            songSearchResults = songController.SearchSongsOnString(new List<string>() { "meTest" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songNameTest.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }
        [Test]
        public void Controller_SongController_SearchOnFullArtist()
        {
            var confirm = false;
            songSearchResults = songController.SearchSongsOnString(new List<string>() { "Tester" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songArtistTest.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void Controller_SongController_SearchOnPartialArtist()
        {
            var confirm = false;
            songSearchResults = songController.SearchSongsOnString(new List<string>() { "ste" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songArtistTest.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void Controller_SongController_SearchOnSearchOnMultipleFullSearchTerms()
        {
            var confirm1 = false;
            var confirm2 = false;

            songSearchResults = songController.SearchSongsOnString(new List<string>() { "SongNameTest", "Tester" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songArtistTest.ID)
                {
                    confirm1 = true;
                }

                if (p.ID == songArtistTest.ID)
                {
                    confirm2 = true;
                }
            }
            Assert.IsTrue(confirm1 && confirm2);
        }

        [Test]
        public void Controller_SongController_SearchOnSearchOnMultiplePartialSearchTerms()
        {
            var confirm1 = false;
            var confirm2 = false;

            songSearchResults = songController.SearchSongsOnString(new List<string>() { "ameTes", "este" });
            foreach (var p in songSearchResults)
            {
                if (p.ID == songArtistTest.ID)
                {
                    confirm1 = true;
                }

                if (p.ID == songArtistTest.ID)
                {
                    confirm2 = true;
                }
            }
            Assert.IsTrue(confirm1 && confirm2);
        }

        [TearDown]
        public void TearDown()
        {
            songController.DeleteItem(songNameTest.ID);
            songController.DeleteItem(songArtistTest.ID);
        }


    }
}
