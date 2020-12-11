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

            CreateUsers();
            if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
            else userList = new UserList();

            Console.WriteLine(userList);

            Console.WriteLine();

            CreateTopics();
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
                            Login((UserMsg)request);
                            break;
                        case "Register":
                            Register((UserMsg)request);
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

            public void Login(UserMsg userMsg)
            {
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    {
                        if (user.Password.Equals(userMsg.Password)) {
                            Console.WriteLine("Success: Login succesfully");
                            user.Comm = comm;
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

            public void Register(UserMsg userMsg)
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
                    _currentUser = new User(userMsg, comm);
                    userList.Add(_currentUser);
                    userList.Serialize();

                    Console.WriteLine("Success: New user added");
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
                }

                foreach (User user in userList)
                {
                    if (user.Username.Equals(_currentUser.Username))
                    {
                        _currentUser.Topic = currentTopic.Title;
                        user.Topic = currentTopic.Title;
                        break;
                    }
                }

                if (currentTopic == null)
                {
                    Net.sendMsg(comm.GetStream(), new Answer(false, "This topic does not exist"));
                    return;
                }

                while (true)
                {
                    Chat chat = (Chat)Net.rcvMsg(comm.GetStream());
                    Console.WriteLine(chat);
                    currentTopic.Chats.Add(chat);

                    foreach (User user in userList)
                    {
                        if (user.Topic == currentTopic.Title && !user.Username.Equals(_currentUser.Username))
                        {
                            Console.WriteLine("Sending chat to " + user.Username);
                            Net.sendMsg(user.Comm.GetStream(), chat);
                        }
                    }
                    topicList.Serialize();
                }
            }
        }

        public static void CreateUsers()
        {
            UserList userList = new UserList
            {
                new User("Bob", "qwe"),
                new User("Seb", "qwe"),
                new User("Léo", "qwe"),
                new User("Pam", "qwe")
            };

            userList.Serialize();
        }

        public static void CreateTopics()
        {
            List<Chat> chatMusique = new List<Chat>
            {
                new Chat(userList[0].Username, "Salut Seb"),
                new Chat(userList[1].Username, "Oh tient, salut bob !"),
                new Chat(userList[0].Username, "Qui d'autre est là ?"),
                new Chat(userList[3].Username, "Il y a moi !"),
                new Chat(userList[1].Username, "Salut Pam !"),
            };

            List<Chat> chatSport = new List<Chat>
            {
                new Chat(userList[2].Username, "Demain c'est biathlon !"),
                new Chat(userList[3].Username, "Oh cool ! Quelle heure ?"),
                new Chat(userList[2].Username, "14H30"),
                new Chat(userList[2].Username, "Euh non, 15H !"),
                new Chat(userList[3].Username, "Super, c'est noté !"),
            };

            TopicList topicList = new TopicList
            {
                new Topic("Music", chatMusique),
                new Topic("Sport", chatSport),
                new Topic("Art"),
                new Topic("Cinema")
            };

            topicList.Serialize();
        }
    }
}
