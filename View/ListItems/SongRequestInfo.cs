using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller;
using Controller.DbControllers;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;
using View.DataContexts;

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
            Song = SongController.Create(DatabaseContext.Instance).GetItem(request.SongID.Value);
            ArtistName = ArtistController.Create(DatabaseContext.Instance).GetItem(Song.Artist).ArtistName;
            Duration = TimeSpan.FromSeconds(Song.Duration);
            ImageSource = Song.PathToImage == null ? "../Assets/null.png" : FileCache.Instance.GetFile(Song.PathToImage);
        }

        public static List<SongRequestInfo> ConvertSongRequestToSongRequestInfo(List<Request> songRequests)
        {
            return (from request in songRequests select new SongRequestInfo(request)).ToList();
        }
    }
}
