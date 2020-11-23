using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class SongController : DbController<Song>
    {
        public SongController(DatabaseContext context) : base(context, context.Songs)
        {
        }

        public void UploadSong(Song song, string localpath)
        {
            string remotePath =  FileTransfer.UploadFile(localpath, "songs/" + Path.GetFileName(localpath));
            song.Path = remotePath;
            CreateItem(song);
        }
        
        public List<Song> SearchSongsOnString(List<string> searchterms)
        {

            var songs = GetList(); /*Context.Songs.AsEnumerable();*/
            List<Song> searchSongs = songs
                .Where(song => searchterms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                               searchterms.Any(s => song.Artist != null && song.Artist.ToLower().Contains(s.ToLower())))
                .Take(8)
                .ToList();
            return searchSongs;
        }
    }
}