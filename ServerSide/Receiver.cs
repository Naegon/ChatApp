using System;
using System.Net.Sockets;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private TcpClient comm;
            private User _currentUser;

            public Receiver(TcpClient client)
            {
                comm = client;
            }

            public void Dispatch()
            {
                var loop = true;
                while (loop)
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
                        case "GetUserList":
                            PrivateMessage();
                            break;
                        case "NewTopic":
                            Console.WriteLine("To be implemented");
                            break;
                        case "Join":
                            DisplayTopic((Demand)request);
                            break;
                        case "Quit":
                            loop = false;
                            break;
                        default:
                            Console.WriteLine("To be implemented");
                            break;
                    }
                }
            }
        }
    }
}
