using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Controller.Proxy;
using Microsoft.EntityFrameworkCore;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using View.ListItems;

namespace Controller.DbControllers
{
    public class AlbumArtistSongsController
    {

        private IDatabaseContext context;
        private SongController songController;
        private ArtistController artistController;
        private AlbumController albumController;
        private DbSet<AlbumArtistSong> set;

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext instance of a database session
         *
         * @returns ArtistController : the proxy with a instance of this controller included
         */
        public static AlbumArtistSongsController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<AlbumArtistSongsController>(new object[] { context }, context);
        }

        protected AlbumArtistSongsController(IDatabaseContext context)
        {
            this.context = context;
            set = context.AlbumArtistSongs;
            songController = SongController.Create(context);
            artistController = ArtistController.Create(context);
            albumController = AlbumController.Create(context);
        }

        public void UploadAlbum(ObservableCollection<AlbumSongInfo> albumSongInfos, Uri image, string title, string description, string artistName, string genre)
        {
            var artistId = (int) artistController.GetArtistIdFromUserId(UserController.CurrentUser.ID);
            var album = new Album {AlbumName = title, Description = description, Genre = genre};
            var requestController = RequestController.Create(DatabaseContext.Instance);
            albumController.CreateItem(album);
            foreach (var albumSongInfo in albumSongInfos)
            {
                var song = new Song
                {
                    Name = albumSongInfo.Title,
                    Artist = (int) artistId,
                    Duration = albumSongInfo.Duration.TotalSeconds,
                    Path = albumSongInfo.File.Name,
                    PathToImage = image != null ? "images/" + image.LocalPath.Split("\\").Last() : null,
                    ProducedBy = albumSongInfo.ProducedBy == "" ? null : albumSongInfo.ProducedBy,
                    WrittenBy = albumSongInfo.WrittenBy == "" ? null : albumSongInfo.WrittenBy,
                    Status = SongStatus.AwaitingApproval
                };
                songController.UploadSong(song, albumSongInfo.File.Name);

                var request = new Request()
                {
                    ArtistName = artistName,
                    UserID = UserController.CurrentUser.ID,
                    RequestType = RequestType.Song,
                    SongID = song.ID
                };

                requestController.CreateItem(request);

                var albumArtistSong = new AlbumArtistSong() {AlbumId = album.ID, ArtistId = artistId, SongId = song.ID};
                set.Add(albumArtistSong);
                if (RealDatabase())
                {
                    context.Entry(albumArtistSong).State = EntityState.Added;
                }
            }
            
            if (!RealDatabase()) return;
            context.SaveChanges();
        }
        public bool RealDatabase()
        {
            return context is DatabaseContext;
        }
    }
}
