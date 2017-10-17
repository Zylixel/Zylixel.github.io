#region

using System;
using System.Net;
using System.Net.Sockets;
using log4net;

#endregion

namespace wServer.networking
{
    internal class PolicyServer
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (PolicyServer));

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
                Log.Error(ex);
            }
        }

        public void Start()
        {
            Log.Info("Starting policy server...");
            try
            {
                _listener.Start();
                _listener.BeginAcceptTcpClient(ServePolicyFile, _listener);
                _started = true;
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                Log.Error(ex);
                Log.Warn("Could not start Socket Policy Server, is port 843 occupied?");
                _started = false;
            }
        }

        public void Stop()
        {
            if (!_started) return;
            Log.Info("Stopping policy server...");
            _listener.Stop();
        }
    }
}