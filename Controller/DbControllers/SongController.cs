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
        public SongController(DatabaseContext context) : base(context, context.Songs)
        {
        }

        public List<Song> SearchSongsOnString(List<string> searchterms)
        {
            
            var songs = Context.Songs.AsEnumerable();
            List<Song> searchSongs = songs
                .Where(song => searchterms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                               searchterms.Any(s => song.Artist != null && song.Artist.ToLower().Contains(s.ToLower())))
                .Take(8)
                .ToList();
            return searchSongs;
        }
    }
}