using System;
using System.Collections.Generic;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using NUnit.Framework;

namespace Tests.Users
{
    [TestFixture]
    public class Controller_User_HasPermission
    {
        private UserController userController;

        [SetUp]
        public void SetUp()
        {
            var mock = new MockDatabaseContext();
            userController = UserController.Create(mock);
            UserController.PermissionsCache = new List<Permission>();


            userController.CreateItem(new User() {ID = 10, Email = "boe@gmail.com", RoleID = 3});
            userController.CreateItem(new User() { ID = 11, Email = "boe2@gmail.com", RoleID = 1 });
        }

        [Test]
        public void User_HasPermission_UploadSong()
        {
            UserController.CurrentUser = userController.GetItem(11);

            Assert.IsNull(FileTransfer.Create(new MockDatabaseContext()).UploadFile("nog iets", "iets anders "));

        }

        [TestCase(10)]
        [TestCase(11, typeof(ArgumentOutOfRangeException), 3)]
        [TestCase(11, typeof(ArgumentOutOfRangeException))]
        public void User_HasPermission_CreatePlaylist(int userId, Type exception = null, int amountOfPlaylists = 1)
        {
            var pc = PlaylistController.Create(new MockDatabaseContext());
            var x=  pc.GetList();
            var startId = 98;

            UserController.CurrentUser = userController.GetItem(userId);
            for (int i = 0; i < amountOfPlaylists; i++)
            {
                pc.CreateItem(new Playlist
                {
                    Name = "2",
                    UserID = UserController.CurrentUser.ID,
                    ID = startId + i
                });
            }
            
            if (exception != null)
                Assert.Throws(exception, () => pc.GetItem(startId));
            else
                Assert.IsNotNull(pc.GetItem(startId + (amountOfPlaylists -1)));
        }
    }
}
