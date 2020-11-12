using System;
using System.IO;

namespace Controller
{
    public class FileCache
    {
        public static FileCache Instance { get; set; }
        public FileCache()
        {
            Instance = this;
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
                if(creation + timeSpan <= DateTime.Now)
                    File.Delete(path);
            }
        }
    }

    public class CachedFile
    {
        public string LocalPath { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
