using System;
using System.Linq;

namespace Communication
{
    public interface Message
    {
        string Action { get; }
        string ToString();

    }

    [Serializable]
    public class User : Message
    {
        private string _action;
        private string _username;
        private string _password;

        string Message.Action { get => _action; }
        public string Username { get => _username; }
        public string Password { get => _password; }

        public User(string action, string username, string password)
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

    [Serializable]
    public class Answer : Message
    {
        private string _action;
        private bool _success;
        private string _message;

        public string Action { get => _action; }
        public bool Success { get => _success; }
        public string Message { get => _message; }

        public Answer(bool error, string message)
        {
            _action = "Message";
            _success = error;
            _message = message;
        }

        public override string ToString()
        {
            return ("[" + _action + "] " + (_success ? "Success: " : "Error: ") + _message);
        }
    }
}
