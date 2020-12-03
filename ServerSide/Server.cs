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

                while (true)
                {
                    Message test = Net.rcvMsg(comm.GetStream());

                    switch (test.GetType().ToString())
                    {
                        case "Communication.User":
                            Console.WriteLine("User");
                            Register((User)test);
                            break;
                        default:
                            Console.WriteLine("To be implemented");
                            break;
                    }
                }
            }

            public void Register(User newUser)
            {
                Console.WriteLine("\nReceiving data: " + newUser.ToString());

                bool isValid = true;
                foreach (User user in userList.all)
                {
                    if (user.Username.Equals(newUser.Username))
                    {
                        Console.WriteLine("Error: An user with that username already exist");
                        Net.sendMsg(comm.GetStream(), new Answer(false, "An user with that username already exist"));
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    Console.WriteLine("Creation of the new user...");
                    userList.Add(newUser);
                    userList.Serialize();

                    Console.WriteLine("Success: New user added");
                    Net.sendMsg(comm.GetStream(), new Answer(true, "New user added"));
                }
            }
        }
    }
}
