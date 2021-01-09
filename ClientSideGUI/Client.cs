using System;
using System.Net.Sockets;
using Communication;

namespace ClientSideGUI
{
    // Each client instance will represent a user connected to a server
    public class Client
    {
        
        internal UserMsg CurrentUser;           // Once logged-in, the current user
        internal bool MessageRunning;   // Used to control while loop in conversations
        public TcpClient Comm { get; }

        public Client(string hostname, int port)
        {
            MessageRunning = false;
            Comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }
    }
}
