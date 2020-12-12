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
            Request privateMessage = new Request(Net.Action.GetUserList);
            Net.sendMsg(Comm.GetStream(), privateMessage);

            UserListMsg userList = (UserListMsg)Net.rcvMsg(Comm.GetStream());
            Console.WriteLine(userList);


            if (userList.Usernames.Count <= 0)
            {
                Console.WriteLine("Back to the topic list ? (Type anything)");
                Console.ReadLine();
                ChooseTopic();
                return;
            }

            Console.WriteLine((userList.Usernames.Count + 1) + ". Back");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (userList.Usernames.Count + 1).ToString()) <= 0));

            int target = Convert.ToInt32(choice);

            if (target == userList.Usernames.Count + 1) ChooseTopic();
            else
            {
                Net.sendMsg(Comm.GetStream(), new Demand(Net.Action.PrivateMessage, userList.Usernames[target - 1]));

                Console.Write("[" + _currentUser.Username + "] ");

                new Thread(SendChat).Start();
                new Thread(RcvChat).Start();
            }
        }
    }
}
