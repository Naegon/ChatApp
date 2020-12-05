using System;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Request : Message
    {
        protected string _action;
        public string Action { get => _action; set => _action = value; }

        public Request()
        {
            _action = "";
        }

        public Request(string action)
        {
            _action = action;
        }

        public override string ToString()
        {
            return "[" + _action + "]";
        }
    }

    [Serializable]
    public class Demand : Request
    {
        private string _title;
        public string Title { get => _title; }

        public Demand(string action, string title) : base(action)
        {
            _title = title;
        }

        public override string ToString()
        {
            return "[" + _action + "] " + _title;
        }
    }

    [Serializable]
    public class User : Request
    {
        private string _username;
        private string _password;

        public string Username { get => _username; }
        public string Password { get => _password; }

        public User(string action, string username, string password) : base(action)
        {
            _action = action;
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return ("[" + _action + "] Username: " + _username + " - Password: " + string.Concat(Enumerable.Repeat("*", _password.Length)));
        }
    }
}
