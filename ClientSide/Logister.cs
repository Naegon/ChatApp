using System;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        public void Login()
        {
            Console.Clear();
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            string password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            UserMsg user = new UserMsg(Net.Action.Login, username, password);
            Net.SendMsg(Comm.GetStream(), user);

            Answer answer = (Answer)Net.RcvMsg(Comm.GetStream());
            Console.WriteLine(answer);

            if (answer.Success)
            {
                _currentUser = user;
                Console.Clear();
                ChooseTopic();
            }

            else
            {
                Console.Write("Try again ? (y/n) ");
                string ans = Console.ReadLine();
                while (!(ans.Equals("y") || ans.Equals("n")))
                {
                    Console.Write("Please type y (yes) or n (no) ");
                    ans = Console.ReadLine();
                }
                if (ans.Equals("y")) Login();
                else Menu();
            }
        }


        public void Register()
        {
            Console.Clear();
            Console.WriteLine("Choose an username:");
            string username = Console.ReadLine();

            Console.WriteLine("Choose a secur password:");
            string password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            UserMsg user = new UserMsg(Net.Action.Register, username, password);
            Net.SendMsg(Comm.GetStream(), user);

            Answer answer = (Answer)Net.RcvMsg(Comm.GetStream());
            Console.WriteLine(answer);

            if (answer.Success)
            {
                _currentUser = user;
                Console.Clear();
                ChooseTopic();
            }

            else
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

                // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                else if (i.KeyChar != '\u0000')
                {
                    pwd += i.KeyChar;
                    Console.Write("*");
                }
            }

            return pwd;
        }
    }
}
