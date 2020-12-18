using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Castle.Core.Internal;
using Controller.Proxy;
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
            return ProxyController.AddToProxy<AlbumController>(new object[] {context}, context);
        }

        protected AlbumController(IDatabaseContext context) : base(context, context.Albums)
        {
        }

        /**
         * Upload an whole album in one fuction
         *
         * @AlbumSongInfos contains the singles that are part of the album
         * @image is a Uri containing the path to the image on the filesystem
         * @title is a album title
         * @description is album discription
         * @genre is the albums genre
         *
         * Creates a request for each song added
         */
        public void UploadAlbum(ObservableCollection<AlbumSongInfo> albumSongInfos, Uri image, string title,
            string description, string genre)
        {
            var artist = ArtistController.Create(Context).GetArtistFromUser(UserController.CurrentUser);
            var pathToImage = image != null ? "images/" + image.LocalPath.Split("\\").Last() : null;

            if (artist == null)
                return;

            CreateItem(new Album 
                { AlbumName = title, Description = description, ArtistID = artist.ID, PathToImage = pathToImage ,Genre = genre });

            foreach (var albumSongInfo in albumSongInfos)
            {
                var song = new Song
                {
                    Name = albumSongInfo.Title,
                    ArtistID = artist.ID,
                    Duration = albumSongInfo.Duration.TotalSeconds,
                    Path = albumSongInfo.File.Name,
                    PathToImage = pathToImage,
                    ProducedBy = albumSongInfo.ProducedBy == "" ? null : albumSongInfo.ProducedBy,
                    WrittenBy = albumSongInfo.WrittenBy == "" ? null : albumSongInfo.WrittenBy,
                    Status = SongStatus.AwaitingApproval
                };
                SongController.Create(Context).UploadSong(song, albumSongInfo.File.Name);

                var request = new Request()
                {
                    ArtistName = artist.ArtistName,
                    UserID = UserController.CurrentUser.ID,
                    RequestType = RequestType.Song,
                    SongID = song.ID
                };

                RequestController.Create(DatabaseContext.Instance).CreateItem(request);
            }

            if (!RealDatabase()) return;
            Context.SaveChanges();
        }

        /**
         * The playlist gets selected on Name, Artist
         * filters out if a album has no songs that are approved
         * @param searchTerms A list of strings containing searchTerms
         *
         * @returns List<Song> : A list of albums based on the searchTerms
         */
        public List<Album> SearchAlbumListOnString(List<string> searchTerms)
        {
            return GetList()
                .Where(album =>
                    (searchTerms.Any(s => album.AlbumName != null && album.AlbumName.ToLower().Contains(s.ToLower())) ||
                     searchTerms.Any(s =>
                         album.Artist.ArtistName.Contains(s.ToLower())))
                    && (!album.Songs.Where(s => s.Status.Equals(SongStatus.Approved)).ToList().IsNullOrEmpty()))
                .ToList();
        }
    }
}