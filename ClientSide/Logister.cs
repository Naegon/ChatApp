using System;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private void Login()
        {
            Console.Clear();
            Console.WriteLine("Enter username:");
            var username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            var password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            var user = new UserMsg(Net.Action.Login, username, password);
            Net.SendMsg(Comm.GetStream(), user);

            var answer = (Answer)Net.RcvMsg(Comm.GetStream());
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
                var ans = Console.ReadLine();
                while (ans != null && !(ans.Equals("y") || ans.Equals("n")))
                {
                    Console.Write("Please type y (yes) or n (no) ");
                    ans = Console.ReadLine();
                }
                if (ans != null && ans.Equals("y")) Login();
                else Menu();
            }
        }


        private void Register()
        {
            Console.Clear();
            Console.WriteLine("Choose an username:");
            var username = Console.ReadLine();

            Console.WriteLine("Choose a secure password:");
            var password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            var user = new UserMsg(Net.Action.Register, username, password);
            Net.SendMsg(Comm.GetStream(), user);

            var answer = (Answer)Net.RcvMsg(Comm.GetStream());
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
                var ans = Console.ReadLine();
                while (ans != null && !(ans.Equals("y") || ans.Equals("n")))
                {
                    Console.Write("Please type y (yes) or n (no) ");
                    ans = Console.ReadLine();
                }
                if (ans != null && ans.Equals("y")) Register();
                else Menu();
            }
        }

        // Used to hide password while typing in
        private static string ReadPassword()
        {
            var pwd = "";
            while (true)
            {
                // Intercept user input
                var i = Console.ReadKey(true);

                // Break the loop and return the data on enter key pressed
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("\n");
                    break;
                }

                // Manage backspace usage
                if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.Remove(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }

                // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                // Print a * for each printable characters
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
