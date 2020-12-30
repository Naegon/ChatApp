using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.Sleep(1000);
            var client = new Client("127.0.0.1", 8976);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var user = new UserMsg(Net.Action.Login, "Bob", "qwe");
            Net.SendMsg(client.Comm.GetStream(), user);
            var answer = (Answer)Net.RcvMsg(client.Comm.GetStream());
            
            if (answer.Success)
            {
                client._currentUser = user;
                Application.Run(new ChooseTopic(client));
            }
            else
            {
                Console.WriteLine("Something went terribly wrong");
            }
            
            // Application.Run(new Menu(client));
        }
    }
}