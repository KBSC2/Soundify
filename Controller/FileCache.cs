using System;
using System.IO;

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
            if (!File.Exists(FileTransfer.RemotePathToLocalPath(path)))
                FileTransfer.DownloadFile(path);
            return FileTransfer.RemotePathToLocalPath(path);
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
