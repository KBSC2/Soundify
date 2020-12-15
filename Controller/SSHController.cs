﻿using System.Threading;
using System.Collections.Generic;
using System.Configuration;
using System;
 using Renci.SshNet;

namespace Controller
{
    public class SSHController
    {
        // Set this to false, if using local Docker MSSQL database
        private bool CreateSSHTunnel = true;
        private static bool running;
        
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
         * Open an SSH tunnel to our Ubuntu machine
         * Start an SSHClient to connect
         */
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

                try
                {
                    // Try to start the port, if port is being used close
                    port.Start();
                }
                catch (Exception)
                {
                    return;
                }

                while(running)
                {
                }
            }
        }

        /**
         * Get the configurations from the App.Config
         *
         * @return Dictionary<string, string> : The configuration for the SSH client
         */
        public static Dictionary<string, string> GetSSHConfiguration()
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
         * Open an SSH-tunnel if no current connection is found
         */
        public void OpenSSHTunnel()
        {
            if (!CreateSSHTunnel)
                return;


            if (thread != null)
                return;

            running = true;
            thread = new Thread(TunnelThread);
            thread.Start();
        }

        public void CloseSSHTunnel()
        {
            running = false;
        }
    }
}
