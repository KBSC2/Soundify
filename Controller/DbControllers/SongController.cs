using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class SongController : DbController<Song>
    {
        public SongController(DatabaseContext context, DbSet<Song> set) : base(context, set)
        {
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