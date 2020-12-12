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
        private int port;
        private static UserList userList;
        private static TopicList topicList;

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            var create = false;
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            if (create) Create.Users();
            if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
            else userList = new UserList();

            Console.WriteLine(userList + "\n");

            if (create) Create.Topics(userList);
            if (File.Exists("TopicList.txt")) topicList = TopicList.Deserialize();
            else topicList = new TopicList();

            Console.WriteLine(topicList);

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("\nConnection established @" + comm);
                new Thread(new Receiver(comm).Dispatch).Start();
            }
        }
    }
}
