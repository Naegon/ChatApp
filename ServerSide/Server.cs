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


        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            if (File.Exists("UserList.txt")) userList = UserList.Deserialize();
            else userList = new UserList();

            Console.WriteLine("User list: ");
            userList.Print();

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

                    switch (message.GetType().ToString())
                    {
                        case "Communication.UserMsg":
                            ManageUser((UserMsg)message);
                            break;
                        default:
                            Console.WriteLine("To be implemented");
                            break;
                    }
                }
            }

            public void ManageUser(UserMsg userMsg)
            {
                if (userMsg.Type.Equals("Register")) {
                    bool isValid = true;
                    foreach (User user in userList.all)
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
                        userList.Add(new User(userMsg.Username, userMsg.Password));
                        userList.Serialize();

                        Console.WriteLine("Success: New user added");
                        Net.sendMsg(comm.GetStream(), new Answer(true, "New user added"));
                    }
                } else
                {
                    foreach (User user in userList.all)
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
            }
        }
    }
}
