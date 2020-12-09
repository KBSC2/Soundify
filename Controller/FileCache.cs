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

        /**
         * gets the local path to the song file. if the song is not downloaded yet it gets downloaded
         *
         * @param path the path where the song is stored remotely
         *
         * @return string : the path where the song is stored locally
         */
        public string GetFile(string path)
        {
            var context = new DatabaseContext();
            if (!File.Exists(FileTransfer.Create(context).RemotePathToLocalPath(path)))
                FileTransfer.Create(context).DownloadFile(path);
            return FileTransfer.Create(context).RemotePathToLocalPath(path);
        }

        /**
         * clears the cache every hour
         *
         * @return void
         */
        public void ClearCache()
        {
            ClearCache(TimeSpan.FromHours(1));
        }

        /**
         * clears the cache based on the input
         *
         * @param Timespan the timespan after which the file needs to be removed
         *
         * @return void
         */
        public void ClearCache(TimeSpan timeSpan)
        {
            foreach (string path in Directory.GetFiles(Path.GetTempPath() + "Soundify", "*.*" , SearchOption.AllDirectories))
            {
                DateTime creation = File.GetCreationTime(path);
                if (creation + timeSpan <= DateTime.Now)
                {
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
}
