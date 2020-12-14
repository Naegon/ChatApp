using System;
using System.Collections.Generic;
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
                    if (user.Comm != null && user.Username != currentUser.Username)
                    {
                        connected.Usernames.Add(user.Username);
                    }
                }

                Net.SendMsg(comm.GetStream(), connected);
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

                currentUser.Topic = buddy.Username;
                Console.WriteLine("Private message with " + buddy.Username);

                bool run = true;
                List<Chat> pending = new List<Chat>();
                while (run)
                {
                    Chat chat;
                    Message message = Net.RcvMsg(comm.GetStream());
                    bool isChat = !message.GetType().Equals(typeof(Request));

                    if (isChat) chat = (Chat)message;
                    else
                    {
                        run = false;
                        currentUser.Topic = "";
                        Console.WriteLine((Request)message);
                        chat = new Chat("Server", currentUser.Username + " left the chat");
                        Net.SendMsg(comm.GetStream(), new Chat("", ""));
                    }

                    Console.WriteLine(chat);

                    if (buddy.Topic.Equals(currentUser.Username))
                    {
                        if (pending.Count > 0)
                        {
                            foreach(Chat pendChat in pending)
                            {
                                Net.SendMsg(buddy.Comm.GetStream(), pendChat);
                            }
                        }
                        Net.SendMsg(buddy.Comm.GetStream(), chat);
                    }
                    else
                    {
                        pending.Add(chat);
                    }
                }
            }
        }
    }
}
