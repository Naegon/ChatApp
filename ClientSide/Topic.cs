using System;
using System.Threading;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private void ChooseTopic()
        {
            Console.Clear();
            Console.WriteLine("Asking for Topic list...");
            Net.sendMsg(Comm.GetStream(), new Request(Net.Action.GetTopicList));

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
            Console.WriteLine((i + 1) + ". Refresh");
            Console.WriteLine((i + 2) + ". Disconnect");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (topicList.Titles.Count + 4).ToString()) <= 0));

            var target = Convert.ToInt32(choice);
            if (target == 1) ChooseUser();
            else if (target == topicList.Titles.Count + 2)
            {
                NewTopic();
            }
            else if (target == topicList.Titles.Count + 3)
            {
                ChooseTopic();
            }
            else if (target == topicList.Titles.Count + 4)
            {
                Console.WriteLine("Disconnecting...");
                Net.sendMsg(Comm.GetStream(), new Request(Net.Action.Disconnect));

                Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());
                if (answer.Success) Menu();
                else
                {
                    Console.WriteLine(answer);
                    Console.Write("Do you want to continue? (Please type anything) ");
                    Console.ReadLine();
                    ChooseTopic();
                }
            }
            else
            {
                Demand choosedTopic = new Demand(Net.Action.Join, topicList.Titles[target - 2]);
                Net.sendMsg(Comm.GetStream(), choosedTopic);

                Topic topic = (Topic)Net.rcvMsg(Comm.GetStream());
                Console.WriteLine(topic);
                Console.Write("[" + _currentUser.Username + "] ");

                _messageRunning = true;
                Thread send = new Thread(SendChat);
                Thread rcv = new Thread(RcvChat);
                send.Start();
                rcv.Start();

                send.Join();
                rcv.Join();
                ChooseTopic();
            }
        }

        private void NewTopic()
        {
            Console.Clear();
            Console.Write("Name of the new Topic: ");
            string topicName = Console.ReadLine();

            if (topicName.Equals("")) ChooseTopic();
            else
            {
                Demand newTopic = new Demand(Net.Action.CreateTopic, topicName);
                Net.sendMsg(Comm.GetStream(), newTopic);

                Answer answer = (Answer)Net.rcvMsg(Comm.GetStream());

                if (!answer.Success)
                {
                    Console.WriteLine(answer);
                    Console.Write("Return to Topic List ? (Type anything) ");
                    Console.ReadLine();
                }

                ChooseTopic();
            }
        }
    }
}
