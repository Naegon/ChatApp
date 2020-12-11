using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private TcpClient _comm;
        private UserMsg _currentUser;

        public TcpClient Comm { get => _comm; set => _comm = value; }

        public Client(string hostname, int port)
        {
            _comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }

        public void Menu()
        {
            Console.WriteLine("\nWelcome!");
            Console.WriteLine("1. Login \n2. Register \n3. Quit");

            String choice;
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
                    break;
                default:
                    Console.WriteLine("There has been an error");
                    break;
            }
        }


        private void SendChat()
        {
            while (true)
            {
                var msg = Console.ReadLine();
                Net.sendMsg(Comm.GetStream(), new Chat(_currentUser.Username, msg));
                Console.Write("[" + _currentUser.Username + "] ");
            }
        }

        private void RcvChat()
        {
            while (true)
            {
                Chat chat = (Chat)Net.rcvMsg(Comm.GetStream());
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(chat);
                Console.Write("[" + _currentUser.Username + "] ");
            }
        }


        public static string ReadPassword()
        {
            string pwd = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("\n");
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.Remove(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd += i.KeyChar;
                    Console.Write("*");
                }
            }
            return pwd;
        }
    }
}
