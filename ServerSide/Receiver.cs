using System;
using System.Net.Sockets;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private readonly TcpClient _comm;
            private User _currentUser;

            public Receiver(TcpClient client)
            {
                _comm = client;
            }

            public void Dispatch()
            {
                var loop = true;

                while (loop)
                {
                    var request = (Request)Net.RcvMsg(_comm.GetStream());
                    Console.WriteLine("\nReceiving data: \n" + request);

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
                            Net.SendMsg(_comm.GetStream(), new TopicListMsg(_topicList));
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
