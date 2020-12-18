using System.Collections.Generic;
using System.IO;
using System.Linq;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class SongController : DbController<Song>
    {

        private ArtistController artistController { get; }

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
         * @param localPath A string with the localPath of where the file is stored
         *
         * @return void
         */
        public void UploadSong(Song song, string localPath)
        {
            song.Path = FileTransfer.Create(Context).UploadFile(localPath, "songs/" + Path.GetFileName(localPath));
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
        public List<Song> SearchSongsOnString(List<string> searchTerms)
        {
            return GetList()
                .Where(song => (searchTerms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                                searchTerms.Any(s => artistController.GetItem(song.ArtistID).ArtistName.ToLower().Contains(s.ToLower()))) &&
                               song.Status != SongStatus.AwaitingApproval)
                .ToList();
        }
    }
}