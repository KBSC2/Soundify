﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DbModels
{
    public class Album : DbModel
    {
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public string PathToImage { get; set; }
        public virtual IList<AlbumArtistSong> AlbumArtistSongs { get; set; }
    }
}
