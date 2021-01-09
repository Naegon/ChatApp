using System;
using System.Collections.Generic;
using System.Linq;
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

                var connected = new UserListMsg();
                foreach (var user in _userList.Where(user => user.Comm != null && user.Username != _currentUser.Username))
                {
                    connected.Usernames.Add(user.Username);
                }

                Net.SendMsg(_comm.GetStream(), connected);
            }

            private void PrivateMessage(Demand demand)
            {
                var buddy = _userList.FirstOrDefault(user => user.Username.Equals(demand.Title));

                _currentUser.Topic = buddy.Username;
                Console.WriteLine("Private message with " + buddy.Username);

                var run = true;
                var pending = new List<Chat>();
                while (run)
                {
                    Chat chat;
                    var message = Net.RcvMsg(_comm.GetStream());
                    var isChat = message.GetType() != typeof(Request);

                    if (isChat) chat = (Chat)message;
                    else
                    {
                        run = false;
                        _currentUser.Topic = "";
                        Console.WriteLine((Request)message);
                        chat = new Chat("Server", _currentUser.Username + " left the chat");
                        Net.SendMsg(_comm.GetStream(), new Chat("", ""));
                    }

                    Console.WriteLine(chat);

                    if (buddy.Topic.Equals(_currentUser.Username))
                    {
                        if (pending.Count > 0)
                        {
                            foreach(var pendChat in pending)
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
