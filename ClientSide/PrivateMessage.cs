using System;
using Communication;

namespace ClientSide
{
    public partial class Client
    {
        private void ChooseUser()
        {
            Request privateMessage = new Request("GetUserList");
            Net.sendMsg(Comm.GetStream(), privateMessage);

            UserListMsg userList = (UserListMsg)Net.rcvMsg(Comm.GetStream());
            Console.WriteLine(userList);
            Console.WriteLine((userList.Usernames.Count + 1) + ". Back");

            string choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!(String.Compare(choice, "1") >= 0 && String.Compare(choice, (userList.Usernames.Count + 1).ToString()) <= 0));

            var target = Convert.ToInt32(choice);

            if (target == userList.Usernames.Count + 1) ChooseTopic();
            else
            {
                Net.sendMsg(Comm.GetStream(), new Demand("privateMesage", userList.Usernames[target - 1]));
            }
        }
    }
}
