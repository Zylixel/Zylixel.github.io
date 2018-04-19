#region

using System;
using System.Globalization;
using System.IO;
using System.Threading;
using db;
using wServer.networking;
using wServer.realm;

#endregion

namespace wServer
{
    internal static class Program
    {
        public static bool WhiteList { get; private set; }
        public static bool Verify { get; private set; }
        public static bool DebugMode { get; private set; }
        public static bool isLagging { get; set; }
        internal static SimpleSettings Settings;

        private static RealmManager manager;

        public static DateTime WhiteListTurnOff { get; private set; }
        public static bool wServerShutdown;

        private static void Main(string[] args)
        {
            Console.Title = "Zy's Realm - World Server";
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.Name = "Entry";

                Settings = new SimpleSettings("wServer");
                new Database(
                    Settings.GetValue<string>("db_host", "127.0.0.1"),
                    Settings.GetValue<string>("db_database", "rotmgprod"),
                    Settings.GetValue<string>("db_user", "root"),
                    Settings.GetValue<string>("db_auth", ""));

                manager = new RealmManager(
                    Settings.GetValue<int>("maxClients", "100"),
                    Settings.GetValue<int>("tps", "20"));

                WhiteList = Settings.GetValue<bool>("whiteList", "false");
                DebugMode = Settings.GetValue<bool>("debugMode", "false");

                manager.Initialize();
                manager.Run();

                Server server = new Server(manager);
                PolicyServer policy = new PolicyServer();

                Console.CancelKeyPress += (sender, e) => e.Cancel = true;

                policy.Start();
                server.Start();
                if (Settings.GetValue<bool>("broadcastNews", "false") && File.Exists("news.txt"))
                    new Thread(autoBroadcastNews).Start();
                Console.WriteLine("Server initialized.");


                if (isStopped())
                {
                    Console.WriteLine("Terminating...");
                    server.Stop();
                    policy.Stop();
                    manager.Stop();
                    Console.WriteLine("Server terminated.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                foreach (var c in manager.Clients)
                {
                    c.Value.Disconnect(Client.DisconnectReason.STOPPING_SERVER);
                }
                Console.ReadLine();
            }
        }

        private static bool isStopped()
        {
            while (!Console.KeyAvailable)
            {
                if (wServerShutdown)
                    return true;
            }
            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                return true;
            return isStopped();
        }

        private static void autoBroadcastNews()
        {
            var news = File.ReadAllLines("news.txt");
            do
            {
                ChatManager cm = new ChatManager(manager);
                cm.News(news[new Random().Next(news.Length)]);
                Thread.Sleep(300000); //5 min
            }
            while (true);
        }

        public static void writeNotable(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        public static void writeWarning(string text)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        public static void writeError(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Blue;
        }
        public static void writeImportant(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Blue;
        }
    }
}