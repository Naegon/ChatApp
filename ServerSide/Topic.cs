using System;
using System.Linq;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void DisplayTopic(Demand demand)
            {
                var currentTopic = new Topic();

                foreach (var topic in _topicList.Where(topic => topic.Title.Equals(demand.Title)))
                {
                    currentTopic = topic;
                    Net.SendMsg(_comm.GetStream(), topic);
                    break;
                }

                _currentUser.Topic = currentTopic.Title;

                var run = true;
                while (run)
                {
                    Chat chat;
                    var message = Net.RcvMsg(_comm.GetStream());
                    var isChat = message.GetType() != typeof(Request);

                    if (isChat) chat = (Chat)message;
                    else
                    {
                        run = false;
                        Console.WriteLine((Request)message);
                        chat = new Chat("Server", _currentUser.Username + " left the chat");
                        _currentUser.Topic = "";
                        Net.SendMsg(_comm.GetStream(), new Chat("", ""));
                    }

                    Console.WriteLine(chat);
                    if (isChat) currentTopic.Chats.Add(chat);

                    foreach (var user in _userList.Where(user => user.Topic == currentTopic.Title && !user.Username.Equals(_currentUser.Username)))
                    {
                        Console.WriteLine("Sending chat to " + user.Username);
                        Net.SendMsg(user.Comm.GetStream(), chat);
                    }
                    if (isChat) _topicList.Serialize();
                }
            }

            private void CreateTopic(Demand newTopic)
            {
                if (_topicList.Any(topic => topic.Title.Equals(newTopic.Title)))
                {
                    Console.WriteLine("A topic with that name already exist");
                    Net.SendMsg(_comm.GetStream(), new Answer(false, "A topic with that name already exist"));
                    return;
                }

                Console.WriteLine("Creating new topic called " + newTopic.Title);

                _topicList.Add(new Topic(newTopic.Title));
                _topicList.Serialize();

                Net.SendMsg(_comm.GetStream(), new Answer(true, "Topic " + newTopic.Title + " successfully created"));
            }
        }
    }
}
