using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;
using db;
using Newtonsoft.Json;

namespace server.admin
{
    public class PerformCommand : RequestHandler
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint pid);
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        private const uint WmKeydown = 0x100;

        protected override void HandleRequest()
        {
            using (var db = new Database())
            {
                var acc = db.Verify(Query["guid"], Query["password"], Program.GameData);

                if (!CheckAccount(acc, db))
                    return;

                if (!acc.Admin)
                {
                    WriteErrorLine("You are not an admin.");
                    return;
                }

                switch (Query["command"])
                {
                    case "init":
                        IntPtr hWnd = FindWindow(null, "Fabiano Swagger of Doom - World Server");
                        bool error;
                        string path = GetProcessPath(hWnd, out error);
                        if (error)
                        {
                            WriteErrorLine("WServer not running.\nApplication will now exit.");
                            return;
                        }
                        WriteLine("NO_SHOW\n{0}\n{1}", Application.ExecutablePath, path);
                        break;
                    case "restartServer":
                        Application.Restart();
                        WriteLine("Server Restarting...");
                        Environment.Exit(0);
                        break;
                    case "reloadServerCFG":
                        Program.Settings.Reload();
                        WriteLine("Settings reloaded.");
                        break;
                    case "restartWServer":
                        hWnd = FindWindow(null, "Fabiano Swagger of Doom - World Server");
                        path = GetProcessPath(hWnd, out error);
                        if(error)
                        {
                            WriteErrorLine(path);
                            return;
                        }
                        SendKeystroke(hWnd, 2 | ((1 | 8) | 16));
                        Process.Start(path);
                        WriteLine("WServer Starting...");
                        break;
                    case "reloadWServerCFG":
                        hWnd = FindWindow(null, "Fabiano Swagger of Doom - World Server");
                        SendKeystroke(hWnd, 2 | 80);
                        WriteLine("Send Reload Settings request to World Server.\nReloading soon.");
                        break;
                    case "stopServer":
                        WriteLine("Stopping...");
                        Environment.Exit(0);
                        break;
                    case "stopWServer":
                        hWnd = FindWindow(null, "Fabiano Swagger of Doom - World Server");
                        SendKeystroke(hWnd, 2 | ((1 | 8) | 16));
                        WriteLine("Stopping WServer...");
                        break;
                    case "startWServer":
                        Process.Start(Query["path"]);
                        WriteLine("Starting World Server...");
                        break;
                    case "startEditWServerCFG":
                        using (var rdr = new StreamReader(File.OpenRead(Query["path"])))
                            WriteLine("NO_SHOW" + rdr.ReadToEnd());
                        break;
                    case "endEditWServerCFG":
                        File.WriteAllText(Query["path"], HttpUtility.UrlDecode(Query["content"]));
                        WriteLine("World server config has been written.");
                        break;
                    case "createPackage":
                        var p = Package.Deserialize(Query["package"]);
                        var cmd = db.CreateQuery();
                        cmd.CommandText = "INSERT INTO packages(name, maxPurchase, weight, contents, bgUrl, price, quantity, endDate) VALUES(@name, @maxPurchase, @weight, @contents, @bgUrl, @price, @quantity, @endDate)";
                        cmd.Parameters.AddWithValue("@name", p.Name);
                        cmd.Parameters.AddWithValue("@maxPurchase", p.MaxPurchase);
                        cmd.Parameters.AddWithValue("@weight", p.Weight);
                        cmd.Parameters.AddWithValue("@contents", p.Content.ToString());
                        cmd.Parameters.AddWithValue("@bgUrl", p.BgUrl);
                        cmd.Parameters.AddWithValue("@price", p.Price);
                        cmd.Parameters.AddWithValue("@quantity", p.Quantity);
                        cmd.Parameters.AddWithValue("@endDate", p.EndDate);
                        cmd.ExecuteNonQuery();
                        break;
                    default:
                        WriteErrorLine("Unknown command: \"{0}\"", Query["command"]);
                        break;
                }
            }
        }

        public static void SendKeystroke(IntPtr hWnd, ushort key)
        {
            SendMessage(hWnd, WmKeydown, ((IntPtr)key), (IntPtr)0);
        }

        public static string GetProcessPath(IntPtr hWnd, out bool error)
        {
            try
            {
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);
                if (Is64Bit(Process.GetProcessById((int)pid).Handle))
                {
                    error = false;
                    return Process.GetProcessById((int)pid).MainModule.FileName;
                }
                return GetMainModuleFilepath((int)pid, out error);
            }
            catch (Exception ex)
            {
                error = true;
                return ex.Message;
            }
        }

        public static string GetMainModuleFilepath(int pid, out bool error)
        {
            try
            {
                string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + pid;
                using (var searcher = new ManagementObjectSearcher(wmiQueryString))
                {
                    using (var results = searcher.Get())
                    {
                        ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                        if (mo != null)
                        {
                            error = false;
                            return (string)mo["ExecutablePath"];
                        }
                    }
                }
                error = false;
                return null;
            }
            catch (Exception ex)
            {
                error = true;
                return ex.Message;
            }
        }

        public static bool Is64Bit(IntPtr hProcess)
        {
            bool retVal;
            IsWow64Process(hProcess, out retVal);
            return retVal;
        }

        struct Package
        {
            public string Name;
            public int MaxPurchase;
            public int Weight;
            public Content Content;
            public string BgUrl;
            public int Price;
            public int Quantity;
            public int EndDate;

            public static Package Deserialize(string json) => new JsonSerializer().Deserialize<Package>(new JsonTextReader(new StringReader(json)));
        }

        struct Content
        {
            public override string ToString()
            {
                var wtr = new StringWriter();
                new JsonSerializer().Serialize(new JsonTextWriter(wtr), this);
                return wtr.ToString();
            }
        }
    }
}
