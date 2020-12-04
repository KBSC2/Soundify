using System.Collections.Generic;
using System.IO;
using Model.Annotations;
using Model.Enums;
using Renci.SshNet;

namespace Controller
{
    public static class FileTransfer
    {
        public static void DownloadFile(string inputPath)
        {
            if(inputPath == "") return;

            var conf = SSHController.GetSSHConfiguration();

            using (ScpClient client = new ScpClient(
                conf.GetValueOrDefault("Host"),
                conf.GetValueOrDefault("Username"),
                conf.GetValueOrDefault("Password")))
            {
                client.Connect();
                string localPath = RemotePathToLocalPath(inputPath);
                using (Stream localFile = File.Create(localPath))
                {
                    client.Download("/home/student/files/" + inputPath, localFile);
                }
            }
        }

        [HasPermission(Permission = Permissions.SongUpload)]
        public static string UploadFile(string inputPath, string outputPath)
        {
            var conf = SSHController.GetSSHConfiguration();
            using (SftpClient client = new SftpClient(
                conf.GetValueOrDefault("Host"),
                conf.GetValueOrDefault("Username"),
                conf.GetValueOrDefault("Password")))
            {
                client.Connect();
                using (Stream localFile = File.OpenRead(inputPath))
                {
                    client.UploadFile(localFile, "/home/student/files/" + outputPath);
                    return outputPath;
                }
            }
        }

        public static string RemotePathToLocalPath(string remotePath)
        {
            return Path.GetTempPath() + "Soundify/" +  remotePath;
        }
    }
}
