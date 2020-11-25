using System.Threading;
using System.Collections.Generic;
using System.Configuration;
using Renci.SshNet;

namespace Controller
{

public class SSHController
    {
        private bool CreateSSHTunnel = true;

        private static SSHController _instance;

        private Thread thread;

        public static SSHController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SSHController();
                return _instance;
            }
        }

        private SSHController()
        {
            _instance = this;
        }

        public void TunnelThread()
        {
            var conf = GetSSHConfiguration();

            using (var client = new SshClient(
                conf.GetValueOrDefault("Host"), 
                conf.GetValueOrDefault("Username"),
                conf.GetValueOrDefault("Password")))
            {
                client.Connect();

                var port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);
                client.AddForwardedPort(port);

                port.Start();

                for (;;)
                {
                }
            }
        }

        /**
         * Get the configurations from the App.Config
         *
         * @return Dictionary<string, string> : 
         */
        public Dictionary<string, string> GetSSHConfiguration()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Configuration configuration = ConfigurationManager.OpenExeConfiguration(@"View.dll");
            foreach (var s in configuration.ConnectionStrings.ConnectionStrings["SSH"].ConnectionString.Split(";"))
            {
                string[] split = s.Trim().Split("=");
                if(split.Length == 2)
                    result.Add(split[0].Trim(), split[1].Trim());
            }

            return result;
        }

        /**
         * Open an SSHtunnel if no current connection is found
         */
        public void OpenSSHTunnel()
        {
            if (!CreateSSHTunnel)
                return;


            if (thread != null)
                return;

            thread = new Thread(TunnelThread);
            thread.Start();
        }
    }
}
