using System;
using System.Collections.Generic;
using System.Text;
using Model.DbModels;

namespace Model.ListItems
{
    public class PlaylistInfo
    {
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public PlaylistInfo(Playlist playlist)
        {
            Name = playlist.Name;
            Genre = playlist.Genre;
            Description = playlist.Description;
            CreationDate = playlist.CreationDate;
        }
    }
}
