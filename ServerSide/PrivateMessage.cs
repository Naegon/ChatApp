using System;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void PrivateMessage()
            {
                Console.WriteLine("Sending back user list");
                Net.sendMsg(comm.GetStream(), new UserListMsg(userList, _currentUser));

                Demand demand = (Demand)Net.rcvMsg(comm.GetStream());

                User buddy = null;
                foreach (User user in userList)
                {
                    if (user.Username.Equals(demand.Title))
                    {
                        buddy = user;
                        break;
                    }
                }

                Console.WriteLine("Private message with " + buddy.Username);
            }
        }
    }
}
