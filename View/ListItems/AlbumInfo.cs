using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;

namespace View.ListItems
{
    public class AlbumInfo
    {
        public Album Album { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set; }
        public string Genre { get; set; }

        public AlbumInfo(Album album, string albumName, string artistName, string genre)
        {
            Album = album;
            AlbumName = albumName;
            ArtistName = artistName;
            Genre = genre;
        }


        public static List<AlbumInfo> ConvertAlbumToAlbumInfo(List<Album> albums)
        {
            var albumInfos = new List<AlbumInfo>();
            foreach (var album in albums)
            {
                var artistId = album.AlbumArtistSongs
                    .Where(aas => aas.Album.ID.Equals(album.ID)).ToList()
                    .GroupBy(aas => aas.ArtistId)
                    .OrderBy(aas => aas.Count())
                    .Select(aas => aas.Key).First();
                var artist = ArtistController.Create(DatabaseContext.Instance).GetItem(artistId);
                albumInfos.Add(new AlbumInfo(album, album.AlbumName, artist.ArtistName, album.Genre));
            }

            return albumInfos;
        }
    }
}