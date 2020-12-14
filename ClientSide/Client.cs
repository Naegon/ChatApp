using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private TcpClient _comm;
        private UserMsg _currentUser;
        private bool _messageRunning = false;

        public TcpClient Comm { get => _comm; set => _comm = value; }

        public Client(string hostname, int port)
        {
            _comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }

        public void Menu()
        {
            Console.Clear();
            Console.WriteLine("\nWelcome!");
            Console.WriteLine("1. Login \n2. Register \n3. Quit");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!choice.Equals("1") && !choice.Equals("2") && !choice.Equals("3"));

            switch (choice)
            {
                case ("1"):
                    Console.WriteLine("Login \n");
                    Login();
                    break;
                case ("2"):
                    Console.WriteLine("Register \n");
                    Register();
                    break;
                case ("3"):
                    Console.WriteLine("Quit");
                    Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Quit));
                    Comm.Close();
                    break;
                default:
                    Console.WriteLine("There has been an error");
                    break;
            }
        }


        private void SendChat()
        {
            while (_messageRunning)
            {
                var msg = Console.ReadLine();
                if (msg.Equals("!quit"))
                {
                    _messageRunning = false;
                    Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Quit));
                }
                else
                {
                    if (!msg.Equals(""))
                    {
                        Net.SendMsg(Comm.GetStream(), new Chat(_currentUser.Username, msg));
                    }
                    Console.Write("[" + _currentUser.Username + "] ");
                }
            }
        }

        private void RcvChat()
        {
            while (_messageRunning)
            {
                Chat chat = (Chat)Net.RcvMsg(Comm.GetStream());
                if (!(chat.Sender.Equals("") && chat.Content.Equals("")))
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.WriteLine(chat);
                    Console.Write("[" + _currentUser.Username + "] ");
                }
            }
        }
    }
}
