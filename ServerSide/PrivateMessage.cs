using System;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void GetUserList()
            {
                Console.WriteLine("Sending back user list");

                UserListMsg connected = new UserListMsg();
                foreach (User user in userList)
                {
                    if (user.Comm != null && user.Username != _currentUser.Username)
                    {
                        connected.Usernames.Add(user.Username);
                    }
                }

                Net.sendMsg(comm.GetStream(), connected);
            }

            private void PrivateMessage(Demand demand)
            {
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

                while (true)
                {
                    Chat chat = (Chat)Net.rcvMsg(comm.GetStream());
                    Console.WriteLine(chat);
                    Net.sendMsg(buddy.Comm.GetStream(), chat);
                }
            }
        }
    }
}
