﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Controller.Proxy;
using Model.Database.Contexts;
using Model.DbModels;
using Model.Enums;

namespace Controller.DbControllers
{
    public class SongController : DbController<Song>
    {

        public static SongController Create(IDatabaseContext context)
        {
            return ProxyController.AddToProxy<SongController>(new object[] { context }, context);
        }

        protected SongController(IDatabaseContext context) : base(context, context.Songs)
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
            return GetList()
                .Where(song => (searchterms.Any(s => song.Name != null && song.Name.ToLower().Contains(s.ToLower())) ||
                               searchterms.Any(s => song.Artist != null && song.Artist.ToLower().Contains(s.ToLower()))) &&
                               song.Status != SongStatus.AwaitingApproval)
                .Take(8)
                .ToList();
        }
    }
}