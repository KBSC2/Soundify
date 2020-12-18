using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model.DbModels;

namespace View.ListItems
{
    public class SongRequestInfo
    {
        public Request Request { get; set; }
        public string ArtistName { get; set; }
        public Song Song { get; set; }
        public TimeSpan Duration { get; set; }
        public string ImageSource { get; set; }

        public SongRequestInfo(Request request)
        {
            if(!request.SongID.HasValue) return;
            Request = request;
            Song = request.Song;
            ArtistName = request.Song.Artist.ArtistName;
            Duration = TimeSpan.FromSeconds(Song.Duration);
            ImageSource = Song.PathToImage == null ? "../Assets/null.png" : FileCache.Instance.GetFile(Song.PathToImage);
        }

        public static List<SongRequestInfo> ConvertSongRequestToSongRequestInfo(List<Request> songRequests)
        {
            return (from request in songRequests select new SongRequestInfo(request)).ToList();
        }
    }
}
