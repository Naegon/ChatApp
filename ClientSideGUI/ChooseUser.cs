using System;
using System.Threading;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class ChooseUser : Form
    {
        private Client _client;

        public ChooseUser(Client client, UserListMsg userList)
        {
            _client = client;
            InitializeComponent(userList);
        }
        
        private void refreshButton_Click(object sender, EventArgs e)
        {
            Request privateMessage = new Request(Net.Action.GetUserList);
            Net.SendMsg(_client.Comm.GetStream(), privateMessage);

            UserListMsg userList = (UserListMsg)Net.RcvMsg(_client.Comm.GetStream());
            if (userList.Usernames.Count <= 0)
            {
                MessageBox.Show("No connected user yet. Please try again later");
                Dispose();
            }
            InitializeComponent(userList);
        }
        
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            if (_client.CurrentUser != null)
            {
                Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Disconnect));
                Answer answer = (Answer)Net.RcvMsg(_client.Comm.GetStream());
                _client.CurrentUser = null;
                Dispose();
            }
        }
        
        private void joinButton_Click(Object sender, EventArgs e, string username)
        {
            Net.SendMsg(_client.Comm.GetStream(), new Demand(Net.Action.PrivateMessage, username));
            _client.MessageRunning = true;

            Conversation conversation = new Conversation(username, _client);
            conversation.Show();
        }
    }
}