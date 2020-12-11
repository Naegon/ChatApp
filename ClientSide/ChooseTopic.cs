using System;
using System.Threading;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        public void ChooseTopic()
        {
            Console.Clear();
            Console.WriteLine("Asking for Topic list...");
            Net.sendMsg(Comm.GetStream(), new Request("GetTopicList"));

            TopicListMsg topicList = (TopicListMsg)Net.rcvMsg(Comm.GetStream());

            int i = 2;
            Console.WriteLine("\nPlease choose one of the listed topic: ");
            Console.WriteLine("1. Private message");
            foreach (string title in topicList.Titles)
            {
                Console.WriteLine(i + ". " + title);
                i++;
            }
            Console.WriteLine(i + ". New Topic");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (topicList.Titles.Count + 2).ToString()) <= 0));

            var target = Convert.ToInt32(choice);
            if (target == 1) ChooseUser();
            else if (target == topicList.Titles.Count + 2)
            {
                Request newTopic = new Request("NewTopic");
                Net.sendMsg(Comm.GetStream(), newTopic);
            }
            else
            {
                Demand choosedTopic = new Demand("Join", topicList.Titles[target - 2]);
                Net.sendMsg(Comm.GetStream(), choosedTopic);

                Topic topic = (Topic)Net.rcvMsg(Comm.GetStream());
                Console.WriteLine(topic);
                Console.Write("[" + _currentUser.Username + "] ");

                new Thread(SendChat).Start();
                new Thread(RcvChat).Start();
            }
        }
    }
}
