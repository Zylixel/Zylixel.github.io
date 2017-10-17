using System;
using System.Windows.Forms;

namespace AdminPanel
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new AdminPanel());
            }
            catch (ObjectDisposedException) { }
        }
    }
}
