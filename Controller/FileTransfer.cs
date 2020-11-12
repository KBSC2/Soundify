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
                using (Stream localfile = File.Create(Path.GetTempPath() + "Soundify/" + inputPath))
                {
                    client.Download("/files/" + inputPath, localfile);
                }
            }
        }

        public static void UploadFile(string inputPath, string outputPath)
        {
            using (ScpClient client = new ScpClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
            {
                client.Connect();
                using (Stream localfile = File.OpenRead(inputPath))
                {
                    client.Upload(localfile, "/files/" + outputPath);
                }
            }
        }
    }
}
