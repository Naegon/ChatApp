using System.Collections.Generic;
using System.Windows.Forms;

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
    }
}