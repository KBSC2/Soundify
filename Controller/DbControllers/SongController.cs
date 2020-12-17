using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class SongController : DbController<Song>
    {

        private ArtistController artistController { get; set; }

        /**
         * Creates a instance of this controller
         * It adds the controller to the proxy
         *
         * @param IDatabaseContext Instance of a database session
         *
         * @returns The proxy with a instance of this controller included
         */
        public static SongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<SongController>(new object[] { context }, context);
        }

        protected SongController(IDatabaseContext context) : base(context, context.Songs)
        {
            artistController = ArtistController.Create(Context);
        }

        /**
         * Uploads a song to the ubuntu filesystem
         *
         * @param song A song data object
         * @param localpath A string with the localpath of where the file is stored
         *
         * @return void
         */
        public void UploadSong(Song song, string localpath)
        {
            string remotePath = FileTransfer.Create(Context).UploadFile(localpath, "songs/" + Path.GetFileName(localpath));
            song.Path = remotePath;
            CreateItem(song);
        }
        
        /**
         * Deletes a song from the ubuntu filesystem
         *
         * @param song A song data object
         *
         * @return void
         */
        public void DeleteSong(Song song)
        {
            FileTransfer.Create(Context).DeleteFile(song.Path);

            var imageUsedElsewhere = GetList().Any(s => s.PathToImage == song.PathToImage && s != song);

            if(!imageUsedElsewhere) FileTransfer.Create(Context).DeleteFile(song.PathToImage);

            DeleteItem(song.ID);
        }

        /**
         * The playlist Gets selected on Name, Artist
         *
         * @param searchTerms A list of strings containing searchTerms
         *
         * @returns List<Song> : A list of maximum 8 songs based on the searchTerms
         */
        public List<Song> SearchSongsOnString(List<string> searchterms)
        {
            return GetList()
                .Where(song => (searchterms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                                searchterms.Any(s => artistController.GetItem(song.ArtistID).ArtistName.ToLower().Contains(s.ToLower()))) &&
                               song.Status != SongStatus.AwaitingApproval)
                .ToList();
        }
    }
}