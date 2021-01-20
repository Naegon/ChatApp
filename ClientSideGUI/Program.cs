using System;
using System.Windows.Forms;

namespace ClientSideGUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            System.Threading.Thread.Sleep(1000);
            var client = new Client("127.0.0.1", 8976);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Menu(client));
        }
    }
}