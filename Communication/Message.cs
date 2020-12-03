using System;
using System.Linq;

namespace Communication
{
    public interface Message
    {
        string ToString();
    }

    [Serializable]
    public class User : Message
    {
        private String _username;
        private String _password;

        public string Username { get => _username; }
        public string Password { get => _password; }

        public User(String username, String password)
        {
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return ("Username: " + _username + " - Password: " + string.Concat(Enumerable.Repeat("*", _password.Length)));
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
            return ((_success ? "Success: " : "Error: ") + _message);
        }
    }
}
