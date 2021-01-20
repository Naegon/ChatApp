using System;
using System.Threading;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private void ChooseUser()
        {
            Console.Clear();
            var privateMessage = new Request(Net.Action.GetUserList);
            Net.SendMsg(Comm.GetStream(), privateMessage);

            var userList = (UserListMsg)Net.RcvMsg(Comm.GetStream());
            Console.WriteLine(userList);


            if (userList.Usernames.Count <= 0)
            {
                Console.WriteLine("Back to the topic list ? (Type anything)");
                Console.ReadLine();
                ChooseTopic();
                return;
            }

            Console.WriteLine(userList.Usernames.Count + 1 + ". Back\n");

            int choice;
            do
            {
                try
                {
                    Console.Write("Please choose an option: ");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    choice = 0;
                }

            } while (choice <= 0 || choice > userList.Usernames.Count + 1);

            if (choice == userList.Usernames.Count + 1) ChooseTopic();
            else
            {
                Net.SendMsg(Comm.GetStream(), new Demand(Net.Action.PrivateMessage, userList.Usernames[choice - 1]));

                Console.Write("[" + _currentUser.Username + "] ");

                _messageRunning = true;
                var send = new Thread(SendChat);
                var rcv = new Thread(RcvChat);
                send.Start();
                rcv.Start();

                send.Join();
                rcv.Join();
                ChooseTopic();
            }
        }
    }
}
