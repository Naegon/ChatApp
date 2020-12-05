using System;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Message
    {
        protected string _action;
        public string Action { get => _action; }

        public Message()
        {
            _action = "";
        }

        public Message(string action)
        {
            _action = action;
        }

        public override string ToString()
        {
            return "[" + _action + "]";
        }
    }

    [Serializable]
    public class User : Message
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

    [Serializable]
    public class Topic : Message
    {
        private string _title;
        public string Title { get => _title; }

        public Topic(string action, string title) : base(action)
        {
            _action = action;
            _title = title;
        }

        public override string ToString()
        {
            return ("[" + _action + "] " + _title);
        }
    }

    [Serializable]
    public class TopicListMsg : Message
    {
        private TopicList _topicList;
        public TopicList TopicList { get => _topicList; }

        public TopicListMsg(string action, TopicList topiclist) : base(action)
        {
            _action = action;
            _topicList = topiclist;
        }

        public override string ToString()
        {
            string _out = "[" + _action + "] Topic list:\n";
            foreach(Topic topic in _topicList.all)
            {
                _out += "  - " + topic.Title + "\n";
            }
            return _out;
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
