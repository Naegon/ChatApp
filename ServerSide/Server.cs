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

        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                new Thread(new Receiver(comm).doOperation).Start();
            }
        }

        class Receiver
        {
            private TcpClient comm;
            public UserList userList;

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {
                if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
                else userList = new UserList();

                Console.WriteLine("User list: ");
                userList.Print();

                User newUser = (User)Net.rcvMsg(comm.GetStream());
                Console.WriteLine("Receiving data: " + newUser.ToString());
                userList.Add(newUser);
                userList.Serialize();

                Net.sendMsg(comm.GetStream(), new Answer(true, "Utilisateur créé"));
            }
        }
    }
}
