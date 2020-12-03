using System;
using System.Linq;

namespace ServerSide
{
    [Serializable]
    public class User
    {
        private string _username;
        private string _password;

        public string Username { get => _username; }
        public string Password { get => _password; }

        public User(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public override string ToString()
        {
            return ("[User] Username: " + _username + " - Password: " + string.Concat(Enumerable.Repeat("*", _password.Length)));
        }
    }
}
