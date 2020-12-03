using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    public class Client
    {
        private TcpClient _comm;
        public TcpClient Comm { get => _comm; set => _comm = value; }

        public Client(string hostname, int port)
        {
            _comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }

        public void Menu()
        {
            Console.WriteLine("Welcome!");
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


        public void Register()
        {
            string username;
            string password;

            Console.WriteLine("Choose an username:");
            username = Console.ReadLine();

            Console.WriteLine("Choose a secur password:");
            password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            Net.sendMsg(Comm.GetStream(), new User(username, password));

            Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());

            Console.WriteLine(answer);

            if (answer.Success)
            {
                Console.WriteLine("Login");
                Menu();
            } else
            {
                Console.Write("Try again ? (y/n) ");
                string ans = Console.ReadLine();
                while (!(ans.Equals("y") || ans.Equals("n")))
                {
                    Console.Write("Please type y (yes) or n (no) ");
                    ans = Console.ReadLine();
                }
                if (ans.Equals("y")) Register();
                else Menu();
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
