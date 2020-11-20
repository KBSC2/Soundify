using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.ControlersTests.SearchTests
{
    [TestFixture]
    public class Controller_Playlist_SearchOnString
    {
        private DatabaseContext context;
        private PlaylistController playlistController;
        private Playlist playlistName;
        private Playlist playlistDescription;
        private Playlist playlistGenre;
        private List<Playlist> playlistsResults;

        [SetUp]
        public void SetUp()
        {
            SSHController.Instance.OpenSSHTunnel();
            context = new DatabaseContext();
            playlistController = new PlaylistController(context);
            playlistName = new Playlist() {Name = "PlaylistNameTest"};
            playlistDescription = new Playlist()
                {Name = "PlaylistDescriptionTest", Description = "I really wanna test this"};
            playlistGenre = new Playlist() {Name = "PlaylistGenreTest", Genre = "Metal"};
            playlistController.CreateItem(playlistName);
            playlistController.CreateItem(playlistDescription);
            playlistController.CreateItem(playlistGenre);
            playlistsResults = new List<Playlist>();
        }

        [Test]
        public void SearchOnFullName()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() {"PlaylistNameTest"});
            foreach (var p in playlistsResults) 
            {
                if (p.ID == playlistName.ID)
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "stNa" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistName.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }
        [Test]
        public void SearchOnFullDescription()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "I really wanna test this" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistDescription.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void SearchOnPartialDescription()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "wanna test" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistDescription.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);

        }

        [Test]
        public void SearchOnFullGenre()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "Metal" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistGenre.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void SearchOnPartialGenre()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "etal" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistGenre.ID)
                {
                    confirm = true;
                }
            }
            Assert.IsTrue(confirm);
        }

        [Test]
        public void SearchOnMultipleFullSearchTerms()
        {
            var confirm1 = false;
            var confirm2 = false;
            var confirm3 = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "PlaylistNameTest", "I really wanna test this","Metal" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistName.ID)
                {
                    confirm1 = true;
                }

                if (p.ID == playlistDescription.ID)
                {
                    confirm2 = true;
                }

                if (p.ID == playlistGenre.ID)
                {
                    confirm3 = true;

                }
            }
            Assert.IsTrue(confirm1 && confirm2 && confirm3 );
        }
        [Test]
        public void SearchOnMultiplePartialSearchTerms()
        {
            var confirm1 = false;
            var confirm2 = false;
            var confirm3 = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "meTest", "test this", "eta" });
            foreach (var p in playlistsResults)
            {
                if (p.ID == playlistName.ID)
                {
                    confirm1 = true;
                }

                if (p.ID == playlistDescription.ID)
                {
                    confirm2 = true;
                }

                if (p.ID == playlistGenre.ID)
                {
                    confirm3 = true;

                }
            }
            Assert.IsTrue(confirm1 && confirm2 && confirm3);
        }


        [TearDown]
        public void TearDown()
        {
            playlistController.DeleteItem(playlistName.ID);
            playlistController.DeleteItem(playlistDescription.ID);
            playlistController.DeleteItem(playlistGenre.ID);
        }
    }
}