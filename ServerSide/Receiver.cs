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
                        case Net.Action.Login:
                            Login((UserMsg)request);
                            break;
                        case Net.Action.Register:
                            Register((UserMsg)request);
                            break;
                        case Net.Action.GetTopicList:
                            Console.WriteLine("Sending back topic list");
                            Net.sendMsg(comm.GetStream(), new TopicListMsg(topicList));
                            break;
                        case Net.Action.GetUserList:
                            GetUserList();
                            break;
                        case Net.Action.PrivateMessage:
                            PrivateMessage((Demand)request);
                            break;
                        case Net.Action.CreateTopic:
                            CreateTopic((Demand)request);
                            break;
                        case Net.Action.Join:
                            DisplayTopic((Demand)request);
                            break;
                        case Net.Action.Disconnect:
                            Disconnect();
                            break;
                        case Net.Action.Quit:
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
