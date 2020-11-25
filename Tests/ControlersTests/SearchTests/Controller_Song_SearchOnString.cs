using System.Collections.Generic;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.ControlersTests.SearchTests
{
    [TestFixture]
    public class Controller_Song_SearchOnString
    {
        private DatabaseContext context;
        private SongController songController;
        private Song songNameTest;
        private Song songArtistTest;
        private List<Song> songSearchResults;

        [SetUp]
        public void SetUp()
        {
            DatabaseContext.TEST_DB = true;
            SSHController.Instance.OpenSSHTunnel();
            context = new DatabaseContext();
            songController = new SongController(context);
            songNameTest = new Song(){Name = "SongNameTest", Artist = "Coder", Duration = 10, Path = "../path"};
            songArtistTest = new Song() { Name = "Ik WIl Testen, liefst ieder kwartier", Artist = "Tester", Duration = 10, Path = "../path" };
            songController.CreateItem(songNameTest);
            songController.CreateItem(songArtistTest);
            songSearchResults = new List<Song>();
        }

        [Test]
        public void SearchOnFullName()
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
        public void SearchOnPartialName()
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
        public void SearchOnFullArtist()
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
        public void SearchOnPartialArtist()
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
        public void SearchOnSearchOnMultipleFullSearchTerms()
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
        public void SearchOnSearchOnMultiplePartialSearchTerms()
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
