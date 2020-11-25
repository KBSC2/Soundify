using System;
using System.Threading;
using Renci.SshNet;

namespace Controller
{

public class SSHController
    {
        // Set this to false, if using local Docker MSSQL database
        private bool CreateSSHTunnel = true;
        

        private Thread thread;

        private static SSHController instance;

        public static SSHController Instance
        {
            get
            {
                if (instance == null)
                    instance = new SSHController();
                return instance;
            }
        }

        /**
         * Open an SSH tunnel to our Ubunut machine
         * Start an SSHClient to connect
         */
        public void TunnelThread()
        {
            using (var client = new SshClient("145.44.235.172", "student", "Sterk_W@chtw00rd2"))
            {
                client.Connect();

                var port = new ForwardedPortLocal("127.0.0.1", 1433, "localhost", 1433);
                client.AddForwardedPort(port);

                try
                {
                    // Try to start the port, if port is being used close
                    port.Start();
                }
                catch (Exception)
                {
                    return;
                }

                for (;;)
                {
                }
            }
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
