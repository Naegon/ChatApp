using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    public class Client
    {
        private TcpClient _comm;
        private User _currentUser;

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

        public void Login()
        {
            string username;
            string password;

            Console.WriteLine("Enter username:");
            username = Console.ReadLine();

            Console.WriteLine("Enter password:");
            password = ReadPassword();

            Console.WriteLine("Sending data to server...");
            User user = new User("Login", username, password);
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
            User user = new User("Register", username, password);
            Net.sendMsg(Comm.GetStream(), user);

            Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());

            Console.WriteLine(answer);

            if (answer.Success)
            {
                Console.WriteLine("Login");
                _currentUser = user;
                ChooseTopic();
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

        public void ChooseTopic()
        {
            Console.WriteLine("Asking for Topic list...");
            Net.sendMsg(Comm.GetStream(), new Request("GetTopicList"));

            TopicListMsg topicList = (TopicListMsg)Net.rcvMsg(Comm.GetStream());

            int i = 1;
            Console.WriteLine("\nPlease choose one of the listed topic: ");
            foreach (string title in topicList.Titles)
            {
                Console.WriteLine(i + ". " + title);
                i++;
            }

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, topicList.Titles.Count.ToString()) <= 0));

            Demand choosedTopic = new Demand("Join", topicList.Titles[Convert.ToInt32(choice) - 1]);
            Net.sendMsg(Comm.GetStream(), choosedTopic);

            Topic topic = (Topic)Net.rcvMsg(Comm.GetStream());
            Console.WriteLine(topic);

            while (true)
            {
                Console.Write("[" + _currentUser.Username + "] ");
                Net.sendMsg(Comm.GetStream(), new Chat(_currentUser, Console.ReadLine()));
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
