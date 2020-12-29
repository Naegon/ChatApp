using System;
using System.Windows.Forms;

namespace ClientSideGUI
{
    public partial class Logister : Form
    {
        private Client _client;
        public Logister(string action, Client client)
        {
            _client = client;
            InitializeComponent(action);
        }
        
        private void buttonBack_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}