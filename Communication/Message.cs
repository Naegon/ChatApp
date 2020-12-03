using System;
using System.Linq;

namespace Communication
{
    public interface Message
    {
        string ToString();
    }

    [Serializable]
    public class UserMsg : Message
    {
        private string _type;
        private string _username;
        private string _password;

        public string Type { get => _type; }
        public string Username { get => _username; }
        public string Password { get => _password; }

        public UserMsg(string type, string username, string password)
        {
            _type = type;
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return ("[UserMsg] Type: " + _type + " - Username: " + _username + " - Password: " + string.Concat(Enumerable.Repeat("*", _password.Length)));
        }
    }

    [Serializable]
    public class Answer : Message
    {
        private bool _success;
        private string _message;

        public bool Success { get => _success; }
        public string Message { get => _message; }

        public Answer(bool error, string message)
        {
            _success = error;
            _message = message;
        }

        public override string ToString()
        {
            return ("[Message] " + (_success ? "Success: " : "Error: ") + _message);
        }
    }
}
