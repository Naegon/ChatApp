using System;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Request : Message
    {
        private Net.Action _action;
        public Net.Action Action { get => _action; set => _action = value; }

        public Request() { }

        public Request(Net.Action action)
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
        public string Title { get; }

        public Demand(Net.Action action, string title) : base(action)
        {
            Title = title;
        }

        public override string ToString()
        {
            return "[" + Action + "] " + Title;
        }
    }

    [Serializable]
    public class UserMsg : Request
    {
        public string Username { get; }
        public string Password { get; }

        public UserMsg(Net.Action action, string username, string password) : base(action)
        {
            Action = action;
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return "[" + Action + "] Username: " + Username + " - Password: " + string.Concat(Enumerable.Repeat("*", Password.Length));
        }
    }
}
