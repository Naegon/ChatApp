using System;
using System.Windows.Forms;
using Communication;

namespace ClientSideGUI
{
    public partial class Menu : Form
    {
        private readonly Client _client;
        public Menu(Client client)
        {
            _client = client;
            InitializeComponent();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Net.SendMsg(_client.Comm.GetStream(), new Request(Net.Action.Quit));
            _client.Comm.Close();
            Dispose();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            var login = new Logister(Net.Action.Login, _client);
            login.Show();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            var register = new Logister(Net.Action.Register, _client);
            register.Show();
        }
    }
}
