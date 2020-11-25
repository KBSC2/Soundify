using System.Collections.Generic;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;
using View.DataContexts;

namespace Tests.SearchTests
{
    [TestFixture]
    public class Controller_Playlist_SearchOnString
    {
        private MockDatabaseContext context;
        private PlaylistController playlistController;
        private Playlist playlistName;
        private Playlist playlistDescription;
        private Playlist playlistGenre;
        private List<Playlist> playlistsResults;

        [SetUp]
        public void SetUp()
        {
            context = new MockDatabaseContext();
            playlistController = new PlaylistController(context);
            DataContext.Instance.CurrentUser = new UserController(context).GetItem(1);
            playlistName = new Playlist() {Name = "PlaylistNameTest", UserID = DataContext.Instance.CurrentUser.ID, ActivePlaylist = true};
            playlistDescription = new Playlist()
                {Name = "PlaylistDescriptionTest", Description = "I really wanna test this", UserID = DataContext.Instance.CurrentUser.ID, ActivePlaylist = true };
            playlistGenre = new Playlist() {Name = "PlaylistGenreTest", Genre = "Metal", UserID = DataContext.Instance.CurrentUser.ID, ActivePlaylist = true };
            playlistController.CreateItem(playlistName);
            playlistController.CreateItem(playlistDescription);
            playlistController.CreateItem(playlistGenre);
            playlistsResults = new List<Playlist>();
        }

        [Test]
        public void Controller_PlaylistController_SearchOnFullName()
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
        public void Controller_PlaylistController_SearchOnPartialName()
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
        public void Controller_PlaylistController_SearchOnFullDescription()
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
        public void Controller_PlaylistController_SearchOnPartialDescription()
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
        public void Controller_PlaylistController_SearchOnFullGenre()
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
        public void Controller_PlaylistController_SearchOnPartialGenre()
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
        public void Controller_PlaylistController_SearchOnMultipleFullSearchTerms()
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
        public void Controller_PlaylistController_SearchOnMultiplePartialSearchTerms()
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