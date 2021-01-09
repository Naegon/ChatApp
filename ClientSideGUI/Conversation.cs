using System;
using System.Threading;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class Conversation : Form
    {
        private Client _client;
        public Conversation(Topic topic, Client client)
        {
            _client = client;
            InitializeComponent(topic.Title);

            Thread rcvChat = new Thread(RcvChat);
            rcvChat.Start();
        }

        public Conversation(string username, Client client)
        {
            _client = client;
            InitializeComponent(username);

            Thread rcvChat = new Thread(RcvChat);
            rcvChat.Start();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Send(sender, null);
            }
        }
        
        private void Send(object sender, EventArgs e)
        {
            Chat msg = new Chat(_client.CurrentUser.Username, textBoxChat.Text);
            Net.SendMsg(_client.Comm.GetStream(), msg);
            topicText.Text += msg + "\r\n";
            textBoxChat.Text = "";
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            _client.MessageRunning = false;        
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Quit));
            Dispose();
        }
        
        private void RcvChat()
        {
            while (_client.MessageRunning)
            {
                var chat = (Chat)Net.RcvMsg(_client.Comm.GetStream());
                
                // Discard empty messages
                if (chat.Sender.Equals("") && chat.Content.Equals("")) continue;

                topicText.Text += chat + "\r\n";
            }
        }
    }
}
