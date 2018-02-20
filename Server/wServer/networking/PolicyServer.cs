#region

using System;
using System.Net;
using System.Net.Sockets;
using wServer.logic;

#endregion

namespace wServer.networking
{
    internal class PolicyServer
    {

        private readonly TcpListener _listener;
        private bool _started;

        public PolicyServer()
        {
            _listener = new TcpListener(IPAddress.Any, 843);
        }

        private static void ServePolicyFile(IAsyncResult ar)
        {
            try
            {
                TcpClient cli = ((TcpListener) ar.AsyncState).EndAcceptTcpClient(ar);
                ((TcpListener) ar.AsyncState)?.BeginAcceptTcpClient(ServePolicyFile, ar.AsyncState);
                NetworkStream s = cli.GetStream();
                NReader rdr = new NReader(s);
                NWriter wtr = new NWriter(s);
                if (rdr.ReadNullTerminatedString() == "<policy-file-request/>")
                {
                    wtr.WriteNullTerminatedString(@"<cross-domain-policy>
     <allow-access-from domain=""*"" to-ports=""*"" />
</cross-domain-policy>");
                    wtr.Write((byte) '\r');
                    wtr.Write((byte) '\n');
                }
                cli.Close();
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Start()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Starting policy server...");
            try
            {
                _listener.Start();
                _listener.BeginAcceptTcpClient(ServePolicyFile, _listener);
                _started = true;
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Could not start Socket Policy Server, is port 843 occupied?");
                _started = false;
            }
        }

        public void Stop()
        {
            if (CheckConfig.IsDebugOn())
                if (!_started) return;
            Console.WriteLine("Stopping policy server...");
            _listener.Stop();
        }
    }
}