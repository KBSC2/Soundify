using System;
using System.IO;
using Model.Database.Contexts;

namespace Controller
{
    public class FileCache
    {
        private static FileCache instance;

        public static FileCache Instance
        {
            get
            {
                if (instance == null)
                    instance = new FileCache();
                return instance;
            }
        }

        private FileCache()
        {
        }

        public string GetFile(string path)
        {
            var context = new DatabaseContext();
            if (!File.Exists(FileTransfer.Create(context).RemotePathToLocalPath(path)))
                FileTransfer.Create(context).DownloadFile(path);
            return FileTransfer.Create(context).RemotePathToLocalPath(path);
        }

        public void ClearCache()
        {
            ClearCache(TimeSpan.FromHours(1));
        }

        public void ClearCache(TimeSpan timeSpan)
        {
            foreach (string path in Directory.GetFiles(Path.GetTempPath() + "Soundify", "*.*" , SearchOption.AllDirectories))
            {
                DateTime creation = File.GetCreationTime(path);
                if (creation + timeSpan <= DateTime.Now)
                    try 
                    {
                        File.Delete(path);
                    }
                    catch (IOException)
                    {
                    }
            }
        }
    }
}
