using System.Threading;
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
            using (var client = new SshClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
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

        public void OpenSSHTunnel()
        {
            if (!CreateSSHTunnel)
                return;


            if (thread != null)
                return;

            thread = new Thread(TunnelThread);
            thread.Start();
        }

        public void CloseSSHTunnel()
        {
            thread.Abort();
        }
    }
}
