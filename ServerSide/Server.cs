using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private readonly int _port;
        private static UserList _userList;
        private static TopicList _topicList;

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            const bool create = false;

            var l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), _port);
            l.Start();

            if (create) Create.Users();
            _userList = File.Exists("UserList.txt") ? UserList.Deserialize() : new UserList();

            Console.WriteLine(_userList + "\n");

            if (create) Create.Topics();
            _topicList = File.Exists("TopicList.txt") ? TopicList.Deserialize() : new TopicList();

            Console.WriteLine(_topicList);

            while (true)
            {
                var comm = l.AcceptTcpClient();
                Console.WriteLine("\nConnection established @" + comm);
                new Thread(new Receiver(comm).Dispatch).Start();
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
