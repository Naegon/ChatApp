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

                _currentUser.Topic = buddy.Username;
                Console.WriteLine("Private message with " + buddy.Username);

                bool run = true;
                List<Chat> pending = new List<Chat>();
                while (run)
                {
                    Chat chat;
                    Message message = Net.rcvMsg(comm.GetStream());
                    bool isChat = !message.GetType().Equals(typeof(Request));

                    if (isChat) chat = (Chat)message;
                    else
                    {
                        run = false;
                        _currentUser.Topic = "";
                        Console.WriteLine((Request)message);
                        chat = new Chat("Server", _currentUser.Username + " left the chat");
                        Net.sendMsg(comm.GetStream(), new Chat("", ""));
                    }

                    Console.WriteLine(chat);

                    if (buddy.Topic.Equals(_currentUser.Username))
                    {
                        if (pending.Count > 0)
                        {
                            foreach(Chat pendChat in pending)
                            {
                                Net.sendMsg(buddy.Comm.GetStream(), pendChat);
                            }
                        }
                        Net.sendMsg(buddy.Comm.GetStream(), chat);
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
