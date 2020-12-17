using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using View.ListItems;

namespace Controller.DbControllers
{
    public class AlbumController : DbController<Album>
    {
        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns ArtistController : the proxy with a instance of this controller included
         */
        public static AlbumController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<AlbumController>(new object[] { context }, context);
        }

        protected AlbumController(IDatabaseContext context) : base(context, context.Albums)
        {
        }

        public void UploadAlbum(ObservableCollection<AlbumSongInfo> albumSongInfos, Uri image, string title, string description, string artistName)
        {
            var artistId = ArtistController.Create(Context).GetArtistIdFromUserId(UserController.CurrentUser.ID);

            if (artistId == null)
                return;


            var album = new Album { AlbumName = title, Description = description, ArtistID = artistId.Value};
            var requestController = RequestController.Create(DatabaseContext.Instance);

            CreateItem(album);
            foreach (var albumSongInfo in albumSongInfos)
            {
                var song = new Song
                {
                    Name = albumSongInfo.Title,
                    ArtistID = (int)artistId,
                    Duration = albumSongInfo.Duration.TotalSeconds,
                    Path = albumSongInfo.File.Name,
                    PathToImage = image != null ? "images/" + image.LocalPath.Split("\\").Last() : null,
                    ProducedBy = albumSongInfo.ProducedBy == "" ? null : albumSongInfo.ProducedBy,
                    WrittenBy = albumSongInfo.WrittenBy == "" ? null : albumSongInfo.WrittenBy,
                    Status = SongStatus.AwaitingApproval
                };
                SongController.Create(Context).UploadSong(song, albumSongInfo.File.Name);

                var request = new Request()
                {
                    ArtistName = artistName,
                    UserID = UserController.CurrentUser.ID,
                    RequestType = RequestType.Song,
                    SongID = song.ID
                };

                requestController.CreateItem(request);
            }

            if (!RealDatabase()) return;
            Context.SaveChanges();
        }
    }
}
