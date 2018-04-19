#region

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using wServer.logic;
using wServer.realm;

#endregion

namespace wServer.networking
{
    internal class Server
    {
        public Server(RealmManager manager)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Manager = manager;
        }

        public Socket Socket { get; private set; }
        public RealmManager Manager { get; private set; }

        public void Start()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Starting server...");
            Socket.Bind(new IPEndPoint(IPAddress.Any, Program.Settings.GetValue<int>("port")));
            Socket.Listen(0xff);
            Socket.BeginAccept(Listen, null);
        }

        private void Listen(IAsyncResult ar)
        {
            Socket skt = null;
            try
            {
                skt = Socket.EndAccept(ar);
            }
            catch (ObjectDisposedException)
            {
            }
            try
            {
                Socket.BeginAccept(Listen, null);
            }
            catch (ObjectDisposedException)
            {
            }
            if (skt != null)
                new Client(Manager, skt);
        }

        public void Stop()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Stoping server...");
            foreach (Client i in Manager.Clients.Values.ToArray())
            {
                i.Save();
                i.Disconnect(Client.DisconnectReason.STOPPING_SERVER);
            }
            Socket.Close();
        }
    }
}