using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller;
using Controller.DbControllers;
using Model.Data;
using Model.DbModels;
using NUnit.Framework;
using View.DataContexts;

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
            DatabaseContext.TEST_DB = true;
            SSHController.Instance.OpenSSHTunnel();
            context = new DatabaseContext();
            playlistController = new PlaylistController(context);
            DataContext.Instance.CurrentUser = new UserController(context).GetItem(818);
            playlistName = new Playlist() {Name = "PlaylistNameTest", UserID = DataContext.Instance.CurrentUser.ID};
            playlistDescription = new Playlist()
                {Name = "PlaylistDescriptionTest", Description = "I really wanna test this", UserID = DataContext.Instance.CurrentUser.ID };
            playlistGenre = new Playlist() {Name = "PlaylistGenreTest", Genre = "Metal", UserID = DataContext.Instance.CurrentUser.ID };
            playlistController.CreateItem(playlistName);
            playlistController.CreateItem(playlistDescription);
            playlistController.CreateItem(playlistGenre);
            playlistsResults = new List<Playlist>();
        }

        [Test]
        public void SearchOnFullName()
        {
            var confirm = false;
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() {"PlaylistNameTest"}, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "stNa" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "I really wanna test this" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "wanna test" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "Metal" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "etal" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "PlaylistNameTest", "I really wanna test this","Metal" }, DataContext.Instance.CurrentUser.ID);
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
            playlistsResults = playlistController.SearchPlayListOnString(new List<string>() { "meTest", "test this", "eta" }, DataContext.Instance.CurrentUser.ID);
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