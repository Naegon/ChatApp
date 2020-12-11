using System;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        public void Login()
        {
            string username;
            string password;

            Console.WriteLine("Enter username:");
            username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            UserMsg user = new UserMsg("Login", username, password);
            Net.sendMsg(Comm.GetStream(), user);

            Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());

            Console.WriteLine(answer);

            if (answer.Success)
            {
                _currentUser = user;
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
            string username;
            string password;

            Console.WriteLine("Choose an username:");
            username = Console.ReadLine();

            Console.WriteLine("Choose a secur password:");
            password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            UserMsg user = new UserMsg("Register", username, password);
            Net.sendMsg(Comm.GetStream(), user);

            Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());

            Console.WriteLine(answer);

            if (answer.Success)
            {
                Console.WriteLine("Login");
                _currentUser = user;
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
    }
}
