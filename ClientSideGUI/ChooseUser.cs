using System;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class ChooseUser : Form
    {
        private readonly Client _client;

        public ChooseUser(Client client, UserListMsg userList)
        {
            _client = client;
            InitializeComponent(userList);
        }
        
        private void refreshButton_Click(object sender, EventArgs e)
        {
            var privateMessage = new Request(Net.Action.GetUserList);
            Net.SendMsg(_client.Comm.GetStream(), privateMessage);

            var userList = (UserListMsg)Net.RcvMsg(_client.Comm.GetStream());
            if (userList.Usernames.Count <= 0)
            {
                MessageBox.Show("No connected user yet. Please try again later");
                Dispose();
            }
            InitializeComponent(userList);
        }
        
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        private void joinButton_Click(string username)
        {
            Net.SendMsg(_client.Comm.GetStream(), new Demand(Net.Action.PrivateMessage, username));
            _client.MessageRunning = true;

            var conversation = new Conversation(username, _client);
            conversation.Show();
        }
    }
}