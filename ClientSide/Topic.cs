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
            Net.SendMsg(Comm.GetStream(), new Request(Net.Action.GetTopicList));

            TopicListMsg topicList = (TopicListMsg)Net.RcvMsg(Comm.GetStream());

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

            int choice;
            int count = topicList.Titles.Count;
            do
            {
                try
                {
                    Console.Write("\nPlease choose an option: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    choice = 0;
                }
                
            } while (choice <=0 || choice > count + 4);


            if (choice == 1) ChooseUser();
            else if (choice == count + 2) NewTopic();
            else if (choice == count + 3) ChooseTopic();
            else if (choice == count + 4) Disconnect();
            else
            {
                Demand choosedTopic = new Demand(Net.Action.Join, topicList.Titles[choice - 2]);
                Net.SendMsg(Comm.GetStream(), choosedTopic);

                Topic topic = (Topic)Net.RcvMsg(Comm.GetStream());
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
                Net.SendMsg(Comm.GetStream(), newTopic);

                Answer answer = (Answer)Net.RcvMsg(Comm.GetStream());

                if (!answer.Success)
                {
                    Console.WriteLine(answer);
                    Console.Write("Return to Topic List ? (Type anything) ");
                    Console.ReadLine();
                }

                ChooseTopic();
            }
        }

        private void Disconnect()
        {
            Console.WriteLine("Disconnecting...");
            Net.SendMsg(Comm.GetStream(), new Request(Net.Action.Disconnect));

            Answer answer = (Answer)Net.RcvMsg(Comm.GetStream());
            if (answer.Success)
            {
                _currentUser = null;
                Menu();
            }
            else
            {
                Console.WriteLine(answer);
                Console.Write("Do you want to continue? (Please type anything) ");
                Console.ReadLine();
                ChooseTopic();
            }
        }
    }
}
