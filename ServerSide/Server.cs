using System;
using System.Collections.Generic;
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
            if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
            else userList = new UserList();

            Console.WriteLine(userList);

            Console.WriteLine();

            //CreateTopics();
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
            private User _currentUser;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {
                while (true)
                {
                    Request request = (Request)Net.rcvMsg(comm.GetStream());
                    Console.WriteLine("\nReceiving data: \n" + request.ToString());

                    switch (request.Action)
                    {
                        case "Login":
                            Login((User)request);
                            break;
                        case "Register":
                            Register((User)request);
                            break;
                        case "GetTopicList":
                            Console.WriteLine("Sending back topic list");
                            Net.sendMsg(comm.GetStream(), new TopicListMsg(topicList));
                            break;
                        case "Join":
                            DisplayTopic((Demand)request);
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
                            _currentUser = user;
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
                    _currentUser = userMsg;
                    Net.sendMsg(comm.GetStream(), new Answer(true, "New user added"));
                }
            }

            public void DisplayTopic(Demand demand)
            {
                Topic currentTopic = new Topic();
                foreach (Topic topic in topicList)
                {
                    if (topic.Title.Equals(demand.Title))
                    {
                        currentTopic = topic;
                        Net.sendMsg(comm.GetStream(), topic);
                        break;
                    }
                    else
                    {
                        Net.sendMsg(comm.GetStream(), new Answer(false, "This topic doeas not exist"));
                        return;
                    }
                }

                while (true)
                {
                    Console.WriteLine(((Chat)Net.rcvMsg(comm.GetStream())).Content);
                    //currentTopic.Chats.Add((Chat)Net.rcvMsg(comm.GetStream()));
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
            List<Chat> chatMusique = new List<Chat>
            {
                new Chat(userList[0], "Salut Seb"),
                new Chat(userList[1], "Oh tient, salut bob !"),
                new Chat(userList[0], "Qui d'autre est là ?"),
                new Chat(userList[3], "Il y a moi !"),
                new Chat(userList[1], "Salut Pam !"),
            };

            TopicList topicList = new TopicList
            {
                new Topic("Music", chatMusique),
                new Topic("Sport"),
                new Topic("Art"),
                new Topic("Cinema")
            };

            topicList.Serialize();
        }
    }
}
