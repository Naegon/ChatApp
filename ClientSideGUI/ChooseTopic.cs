using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class ChooseTopic : Form
    {
        private Client _client;
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
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Disconnect));
            Answer answer = (Answer)Net.RcvMsg(_client.Comm.GetStream());
            _client._currentUser = null;
            Dispose();
        }

        private void newTopicButton_Click(Object sender, EventArgs e)
        {
            string value = "Test";
            if (Tmp.InputBox("New topic", "New topic name:", ref value) == DialogResult.OK)
            {
                Demand newTopic = new Demand(Net.Action.CreateTopic, value);
                Net.SendMsg(_client.Comm.GetStream(), newTopic);
            }
            
            Net.RcvMsg(_client.Comm.GetStream());
            InitializeComponent();
        }
        
        private void joinButton_Click(Object sender, EventArgs e, string title)
        {
            Demand choosedTopic = new Demand(Net.Action.Join, title);
            Net.SendMsg(_client.Comm.GetStream(), choosedTopic);
            
            Topic topic = (Topic)Net.RcvMsg(_client.Comm.GetStream());
            _client._messageRunning = true;
            
            Conversation conversation = new Conversation(topic, _client);
            conversation.Show();
            // Thread send = new Thread(SendChat);
            // Thread rcv = new Thread(RcvChat);
            // send.Start();
            // rcv.Start();
            //
            // send.Join();
            // rcv.Join();
            // ChooseTopic();
        }
    }
}
