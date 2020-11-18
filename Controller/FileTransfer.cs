using System.IO;
using Renci.SshNet;

namespace Controller
{
    public static class FileTransfer
    {
        public static void DownloadFile(string inputPath)
        {
            if (!Directory.Exists(Path.GetTempPath() + "Soundify"))
                Directory.CreateDirectory(Path.GetTempPath() + "Soundify");

            using (ScpClient client = new ScpClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
            {
                client.Connect();
                string localpath = RemotePathToLocalPath(inputPath);
                using (Stream localfile = File.Create(localpath))
                {
                    client.Download("/files/" + inputPath, localfile);
                }
            }
        }

        public static string UploadFile(string inputPath, string outputPath)
        {
            using (SftpClient client = new SftpClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
            {
                client.Connect();
                using (Stream localfile = File.OpenRead(inputPath))
                {
                    client.UploadFile(localfile, "/home/student/files/" + outputPath);
                    return outputPath;
                }
            }
        }

        public static string RemotePathToLocalPath(string remotePath)
        {
            return Path.GetTempPath() + "Soundify/" + remotePath;
        }
    }
}
