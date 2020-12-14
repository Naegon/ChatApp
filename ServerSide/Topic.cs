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
                        Net.SendMsg(comm.GetStream(), topic);
                        break;
                    }
                }

                currentUser.Topic = currentTopic.Title;

                if (currentTopic == null)
                {
                    Net.SendMsg(comm.GetStream(), new Answer(false, "This topic does not exist"));
                    return;
                }

                bool run = true;
                while (run)
                {
                    Chat chat;
                    bool isChat;

                    Message message = Net.RcvMsg(comm.GetStream());
                    isChat = !message.GetType().Equals(typeof(Request));

                    if (isChat) chat = (Chat)message;
                    else
                    {
                        run = false;
                        Console.WriteLine((Request)message);
                        chat = new Chat("Server", currentUser.Username + " left the chat");
                        currentUser.Topic = "";
                        Net.SendMsg(comm.GetStream(), new Chat("", ""));
                    }

                    Console.WriteLine(chat);
                    if (isChat) currentTopic.Chats.Add(chat);

                    foreach (User user in userList)
                    {
                        if (user.Topic == currentTopic.Title && !user.Username.Equals(currentUser.Username))
                        {
                            Console.WriteLine("Sending chat to " + user.Username);
                            Net.SendMsg(user.Comm.GetStream(), chat);
                        }
                    }
                    if (isChat) topicList.Serialize();
                }
            }

            private void CreateTopic(Demand newTopic)
            {
                foreach (Topic topic in topicList)
                {
                    if (topic.Title.Equals(newTopic.Title))
                    {
                        Console.WriteLine("A topic with that name already exist");
                        Net.SendMsg(comm.GetStream(), new Answer(false, "A topic with that name already exist"));
                        return;
                    }
                }

                Console.WriteLine("Creating new topic called " + newTopic.Title);

                topicList.Add(new Topic(newTopic.Title));
                topicList.Serialize();

                Net.SendMsg(comm.GetStream(), new Answer(true, "Topic " + newTopic.Title + " succesfully created"));
            }
        }
    }
}
