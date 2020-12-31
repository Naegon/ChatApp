using System;
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
            Console.WriteLine(topic);
            InitializeComponent(topic);
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
            Chat msg = new Chat(_client._currentUser.Username, textBoxChat.Text);
            Net.SendMsg(_client.Comm.GetStream(), msg);
            topicText.Text += msg + "\r\n";
            textBoxChat.Text = "";
        }
    }
}
