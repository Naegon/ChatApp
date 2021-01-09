using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    // Each client instance will represent a user connected to a server
    public partial class Client
    {
        private UserMsg _currentUser;           // Once logged-in, the current user
        private bool _messageRunning;   // Used to control while loop in conversations

        private TcpClient Comm { get; }

        public Client(string hostname, int port)
        {
            _messageRunning = false;
            Comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }

        // Menu used to either login, register or quit the program
        public void Menu()
        {
            string choice;
            
            Console.Clear();
            Console.WriteLine("\nWelcome!");
            Console.WriteLine("1. Login \n2. Register \n3. Quit");
            
            // Prompt user for a valid choice
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (choice != null && !choice.Equals("1") && !choice.Equals("2") && !choice.Equals("3"));

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Login \n");
                    Login();
                    break;
                case "2":
                    Console.WriteLine("Register \n");
                    Register();
                    break;
                case "3":
                    Console.WriteLine("Quit");
                    Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Quit));
                    Comm.Close();
                    break;
                default:
                    Console.WriteLine("There has been an error");
                    break;
            }
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
