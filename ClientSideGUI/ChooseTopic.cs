using System;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class ChooseTopic : Form
    {
        private readonly Client _client;
        public ChooseTopic(Client client)
        {
            _client = client;
            InitializeComponent();
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            InitializeComponent();
        }
        
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            if (_client.CurrentUser == null) return;
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Disconnect));
            var answer = (Answer)Net.RcvMsg(_client.Comm.GetStream());
            _client.CurrentUser = null;
            Dispose();
        }

        private void newTopicButton_Click(object sender, EventArgs e)
        {
            var value = "";
            if (Tmp.InputBox("New topic", "New topic name:", ref value) == DialogResult.OK)
            {
                var newTopic = new Demand(Net.Action.CreateTopic, value);
                Net.SendMsg(_client.Comm.GetStream(), newTopic);
            }
            
            Net.RcvMsg(_client.Comm.GetStream());
            InitializeComponent();
        }
        
        private void joinButton_Click(string title)
        {
            var choosedTopic = new Demand(Net.Action.Join, title);
            Net.SendMsg(_client.Comm.GetStream(), choosedTopic);
            
            var topic = (Topic)Net.RcvMsg(_client.Comm.GetStream());
            _client.MessageRunning = true;
            
            var conversation = new Conversation(topic, _client);
            conversation.Show();
            conversation.topicText.Text = topic.ToString().Replace("\n", "\r\n");
        }
        
        private void privateButton_Click(object sender, EventArgs e)
        {
            var privateMessage = new Request(Net.Action.GetUserList);
            Net.SendMsg(_client.Comm.GetStream(), privateMessage);

            var userList = (UserListMsg)Net.RcvMsg(_client.Comm.GetStream());
            if (userList.Usernames.Count <= 0) MessageBox.Show("No connected user yet. Please try again later");
            else new ChooseUser(_client, userList).Show();
        }
    }
}
