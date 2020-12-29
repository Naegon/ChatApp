using System;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class Menu : Form
    {
        private Client _client;
        public Menu(Client client)
        {
            _client = client;
            InitializeComponent();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Quit));
            _client.Comm.Close();
            Parent.Dispose();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Logister login = new Logister("Login", _client);
            login.Show();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            Logister register = new Logister("Register", _client);
            register.Show();
        }
    }
}
