using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication;

namespace ServerSide
{
    public class Server
    {
        private int port;
        private static UserList userList;
        private static TopicList topicList;

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            //CreateUsers();
            //CreateTopics();

            if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
            else userList = new UserList();

            Console.WriteLine(userList);

            Console.WriteLine();

            if (File.Exists("TopicList.txt")) topicList = TopicList.Deserialize();
            else topicList = new TopicList();

            Console.WriteLine(topicList);

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("\nConnection established @" + comm);
                new Thread(new Receiver(comm).doOperation).Start();
            }
        }

        class Receiver
        {
            private TcpClient comm;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {
                while (true)
                {
                    Message message = Net.rcvMsg(comm.GetStream());
                    Console.WriteLine("\nReceiving data: \n" + message.ToString());

                    switch (message.Action)
                    {
                        case "Login":
                            Login((User)message);
                            break;
                        case "Register":
                            Register((User)message);
                            break;
                        case "GetTopicList":
                            Console.WriteLine("Sending back topic list");
                            Net.sendMsg(comm.GetStream(), new TopicListMsg("Answer", topicList));
                            break;
                        default:
                            Console.WriteLine("To be implemented");
                            break;
                    }
                }
            }

            public void Login(User userMsg)
            {
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    {
                        if (user.Password.Equals(userMsg.Password)) {
                            Console.WriteLine("Success: Login succesfully");
                            Net.sendMsg(comm.GetStream(), new Answer(true, "Login succesfully"));
                        }
                        else
                        {
                            Console.WriteLine("Error: Wrong password");
                            Net.sendMsg(comm.GetStream(), new Answer(false, "Wrong password"));
                        }
                        return;
                    }
                }
                Console.WriteLine("Error: No user with that username");
                Net.sendMsg(comm.GetStream(), new Answer(false, "No user with that username"));
            }

            public void Register(User userMsg)
            {
                bool isValid = true;
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    {
                        Console.WriteLine("Error: An user with that username already exist");
                        Net.sendMsg(comm.GetStream(), new Answer(false, "An user with that username already exist"));
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    Console.WriteLine("Creation of the new user...");
                    userList.Add(userMsg);
                    userList.Serialize();

                    Console.WriteLine("Success: New user added");
                    Net.sendMsg(comm.GetStream(), new Answer(true, "New user added"));
                }
            }
        }

        public static void CreateUsers()
        {
            UserList userList = new UserList
            {
                new User("None", "Bob", "qwe"),
                new User("None", "Seb", "qwe"),
                new User("None", "Léo", "qwe"),
                new User("None", "Pam", "qwe")
            };

            userList.Serialize();
        }

        public static void CreateTopics()
        {
            TopicList topicList = new TopicList
            {
                new Topic("None", "Music"),
                new Topic("None", "Sport"),
                new Topic("None", "Art"),
                new Topic("None", "Cinema")
            };

            topicList.Serialize();
        }
    }
}
