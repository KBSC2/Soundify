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
        /**
         * This function creates a instance of this controller
         * It adds the controller to the proxy
         * @returns the proxy with a instance of this controller included
         */
        public static SongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<SongController>(new object[] { context }, context);
        }

        protected SongController(IDatabaseContext context) : base(context, context.Songs)
        {
        }

        /**
         * uploads a song to the ubuntu filesystem
         */
        public void UploadSong(Song song, string localpath)
        {
            string remotePath =  FileTransfer.Create(Context).UploadFile(localpath, "songs/" + Path.GetFileName(localpath));
            song.Path = remotePath;
            CreateItem(song);
        }

        /**
         * @param searchTerms 
         * @returns a list of maximum 8 songs based on the searchTerms
         * The playlist gets selected on Name, Artist
         */
        public List<Song> SearchSongsOnString(List<string> searchterms)
        {
            return GetList()
                .Where(song => (searchterms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                               searchterms.Any(s => song.Artist != null && song.Artist.ToLower().Contains(s.ToLower()))) &&
                               song.Status != SongStatus.AwaitingApproval)
                .Take(8)
                .ToList();
        }
    }
}