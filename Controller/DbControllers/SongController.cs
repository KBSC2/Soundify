using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.DbModels;

namespace Controller.DbControllers
{
    public class SongController: DbController<Song>
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
    }
}
