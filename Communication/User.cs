using System;
using System.Linq;
using System.Net.Sockets;

namespace Communication
{
    [Serializable]
    public class User
    {
        private string _topic;
        [NonSerialized] private TcpClient _comm;

        public string Username { get; }
        public string Password { get; }
        public string Topic { get => _topic; set => _topic = value; }
        public TcpClient Comm { get => _comm; set => _comm = value; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            _topic = "";
            _comm = null;
        }

        public User(UserMsg userMsg, TcpClient comm)
        {
            Username = userMsg.Username;
            Password = userMsg.Password;
            _topic = "";
            _comm = comm;
        }

        public override string ToString()
        {
            return Username + " - Password: " +
                string.Concat(Enumerable.Repeat("*", Password.Length)) +
                (!_topic.Equals("") ? " - Current Topic: " : "") + _topic;
        }
    }
}
