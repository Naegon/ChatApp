using System;
using System.Net.Sockets;
using Communication;

namespace ClientSideGUI
{
    // Each client instance will represent a user connected to a server
    public class Client
    {
        internal UserMsg _currentUser;           // Once logged-in, the current user
        private bool _messageRunning;   // Used to control while loop in conversations
        public TcpClient Comm { get; }

        public Client(string hostname, int port)
        {
            _messageRunning = false;
            Comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }

        // Used in separate thread to send message to conversations
        private void SendChat()
        {
            while (_messageRunning)
            {
                var msg = Console.ReadLine();
                
                // Check for exit condition
                if (msg != null && msg.Equals("!quit"))
                {
                    // Break the loop then notify server to stop receiving messages
                    _messageRunning = false;        
                    Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Quit));
                }
                else
                {
                    // Send non-empty messages to the server
                    if (msg != null && !msg.Equals(""))
                    {
                        Net.SendMsg(Comm.GetStream(), new Chat(_currentUser.Username, msg));
                    }
                    Console.Write("[" + _currentUser.Username + "] ");
                }
            }
        }
        
        // Used in separate thread to receive message from conversations
        private void RcvChat()
        {
            while (_messageRunning)
            {
                var chat = (Chat)Net.RcvMsg(Comm.GetStream());
                
                // Discard empty messages
                if (chat.Sender.Equals("") && chat.Content.Equals("")) continue;

                // Some formatting of the received message
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(chat);
                Console.Write("[" + _currentUser.Username + "] ");
            }
        }
    }
}
