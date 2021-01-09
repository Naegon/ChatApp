using System;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class Logister : Form
    {
        private Client _client;
        private Net.Action _action;
        
        public Logister(Net.Action action, Client client)
        {
            _client = client;
            _action = action;
            InitializeComponent();
        }
        
        private void buttonBack_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            var user = new UserMsg(_action, textBoxUsername.Text, textBoxPassword.Text);
            Net.SendMsg(_client.Comm.GetStream(), user);

            var answer = (Answer)Net.RcvMsg(_client.Comm.GetStream());
            
            if (answer.Success)
            {
                _client.CurrentUser = user;
                var chooseTopic = new ChooseTopic(_client);
                chooseTopic.Show();
                Dispose();
            }

            else
            {
                labelError.Text = answer.Message;
                labelError.Visible = true;
            }
        }
    }
}