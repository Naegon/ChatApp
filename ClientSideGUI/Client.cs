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

        // Used in separate thread to send message to conversations
        private void SendChat()
        {
            while (MessageRunning)
            {
                var msg = Console.ReadLine();
                
                // Check for exit condition
                if (msg != null && msg.Equals("!quit"))
                {
                    // Break the loop then notify server to stop receiving messages
                    MessageRunning = false;        
                    Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Quit));
                }
                else
                {
                    // Send non-empty messages to the server
                    if (msg != null && !msg.Equals(""))
                    {
                        Net.SendMsg(Comm.GetStream(), new Chat(CurrentUser.Username, msg));
                    }
                    Console.Write("[" + CurrentUser.Username + "] ");
                }
            }
        }
        
        // Used in separate thread to receive message from conversations
        private void RcvChat()
        {
            while (MessageRunning)
            {
                var chat = (Chat)Net.RcvMsg(Comm.GetStream());
                
                // Discard empty messages
                if (chat.Sender.Equals("") && chat.Content.Equals("")) continue;

                // Some formatting of the received message
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(chat);
                Console.Write("[" + CurrentUser.Username + "] ");
            }
        }
    }
}
