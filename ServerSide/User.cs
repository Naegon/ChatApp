using System;
using System.Linq;
using System.Net.Sockets;
using Communication;

namespace ServerSide
{
    [Serializable]
    public class User
    {
        private string _username;
        private string _password;
        private string _topic;
        [NonSerialized] private TcpClient _comm;

        public string Username { get => _username; }
        public string Password { get => _password; }
        public string Topic { get => _topic; set => _topic = value; }
        public TcpClient Comm { get => _comm; set => _comm = value; }

        public User(string username, string password)
        {
            _username = username;
            _password = password;
            _topic = "";
            _comm = null;
        }

        public User(UserMsg userMsg, TcpClient comm)
        {
            _username = userMsg.Username;
            _password = userMsg.Password;
            _topic = "";
            _comm = comm;
        }

        public override string ToString()
        {
            return ("Username: " + _username +
                " - Password: " + string.Concat(Enumerable.Repeat("*", _password.Length)) +
                (!_topic.Equals("") ? " - Current Topic: " : "") + _topic);
        }
    }
}
