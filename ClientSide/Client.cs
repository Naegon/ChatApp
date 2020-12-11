using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Communication;

namespace ClientSide
{
    public class Client
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

            int i = 2;
            Console.WriteLine("\nPlease choose one of the listed topic: ");
            Console.WriteLine("1. Private message");
            foreach (string title in topicList.Titles)
            {
                Console.WriteLine(i + ". " + title);
                i++;
            }
            Console.WriteLine(i + ". New Topic");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (topicList.Titles.Count + 2).ToString()) <= 0));

            var target = Convert.ToInt32(choice);
            if (target == 1) ChooseUser();
            if (target == topicList.Titles.Count + 2)
            {
                Request privateMessage = new Request("NewUser");
                Net.sendMsg(Comm.GetStream(), privateMessage);
            }
            else
            {
                Demand choosedTopic = new Demand("Join", topicList.Titles[target - 2]);
                Net.sendMsg(Comm.GetStream(), choosedTopic);

                Topic topic = (Topic)Net.rcvMsg(Comm.GetStream());
                Console.WriteLine(topic);
            }


            Console.Write("[" + _currentUser.Username + "] ");
            new Thread(SendChat).Start();
            new Thread(RcvChat).Start();
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


        private void ChooseUser()
        {
            Request privateMessage = new Request("GetUserList");
            Net.sendMsg(Comm.GetStream(), privateMessage);

            UserListMsg userList = (UserListMsg)Net.rcvMsg(Comm.GetStream());
            Console.WriteLine(userList);
            Console.WriteLine((userList.Usernames.Count + 1) + ". Back");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (userList.Usernames.Count + 1).ToString()) <= 0));

            var target = Convert.ToInt32(choice);

            if (target == userList.Usernames.Count + 1)  Menu();
            else
            {
                Net.sendMsg(Comm.GetStream(), new Demand("privateMesage", userList.Usernames[target + 1]));
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
