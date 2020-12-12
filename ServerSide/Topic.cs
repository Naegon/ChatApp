using System;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void DisplayTopic(Demand demand)
            {
                Topic currentTopic = new Topic();
                foreach (Topic topic in topicList)
                {
                    if (topic.Title.Equals(demand.Title))
                    {
                        currentTopic = topic;
                        Net.sendMsg(comm.GetStream(), topic);
                        break;
                    }
                }

                foreach (User user in userList)
                {
                    if (user.Username.Equals(_currentUser.Username))
                    {
                        _currentUser.Topic = currentTopic.Title;
                        user.Topic = currentTopic.Title;
                        break;
                    }
                }

                if (currentTopic == null)
                {
                    Net.sendMsg(comm.GetStream(), new Answer(false, "This topic does not exist"));
                    return;
                }

                while (true)
                {
                    Chat chat = (Chat)Net.rcvMsg(comm.GetStream());
                    Console.WriteLine(chat);
                    currentTopic.Chats.Add(chat);

                    foreach (User user in userList)
                    {
                        if (user.Topic == currentTopic.Title && !user.Username.Equals(_currentUser.Username))
                        {
                            Console.WriteLine("Sending chat to " + user.Username);
                            Net.sendMsg(user.Comm.GetStream(), chat);
                        }
                    }
                    topicList.Serialize();
                }
            }

            private void CreateTopic(Demand newTopic)
            {
                Console.WriteLine(newTopic);
                foreach (Topic topic in topicList)
                {
                    if (topic.Title.Equals(newTopic.Title))
                    {
                        Console.WriteLine("A topic with that name already exist");
                        Net.sendMsg(comm.GetStream(), new Answer(false, "A topic with that name already exist"));
                        return;
                    }
                }

                Console.WriteLine("Creating new topic called " + newTopic.Title);

                topicList.Add(new Topic(newTopic.Title));
                topicList.Serialize();

                Net.sendMsg(comm.GetStream(), new Answer(true, "Topic " + newTopic.Title + " succesfully created"));
            }
        }
    }
}
