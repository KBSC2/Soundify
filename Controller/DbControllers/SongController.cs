using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
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
            var songs = Context.Songs.AsEnumerable();
            List<Song> searchSongs = songs
                .Where(song => searchterms.Any(s => song.Name != null && song.Name.Contains(s)) ||
                               searchterms.Any(s => song.Artist != null && song.Artist.Contains(s)))
                .ToList();
            return searchSongs;
        }
    }
}