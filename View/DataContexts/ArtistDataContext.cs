using System;
using System.Collections.Generic;
using System.Text;
using Soundify;

namespace View.DataContexts
{
    public class ArtistDataContext
    {
        private static ArtistDataContext _instance;
        public static ArtistDataContext Instance => _instance ??= new ArtistDataContext();

        public string SelectedSongPath { get; set; }
    }
}
