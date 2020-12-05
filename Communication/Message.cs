using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Message
    {
        protected string _action;
        public string Action { get => _action; set => _action = value; }

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
        private List<Chat> _chats;

        public string Title { get => _title; }
        public List<Chat> Chats { get => _chats; }

        public Topic(string action, string title) : base(action)
        {
            _action = action;
            _title = title;
            _chats = new List<Chat>();
        }

        public Topic(string action, string title, List<Chat> chats) : base(action)
        {
            _action = action;
            _title = title;
            _chats = chats;
        }

        public override string ToString()
        {
            string strOut = _title + " (" + _chats.Count + " messages)";
            _chats.ForEach(chat => strOut += chat);
            return strOut;
        }
    }

    [Serializable]
    public class TopicListMsg : Message
    {
        private List<string> _titles;
        public List<string> Titles { get => _titles; }

        public TopicListMsg(string action, TopicList topicList) : base(action)
        {
            _action = action;
            _titles = new List<string>();
            foreach(Topic topic in topicList)
            {
                _titles.Add(topic.Title);
            }
        }

        public override string ToString()
        {
            string _out = "[" + _action + "] Topic list:\n";
            foreach(string title in _titles)
            {
                _out += "  - " + title + "\n";
            }
            return _out;
        }
    }

    [Serializable]
    public class Chat : Message
    {
        private User _sender;
        private string _content;

        public User Sender { get => _sender; }
        public string Content { get => _content; }

        public Chat(string action, User sender, string content) : base (action)
        {
            _action = action;
            _sender = sender;
            _content = content;
        }

        public override string ToString()
        {
            return "\n[" + _sender.Username + "] " + _content;
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

        public Answer(string action, bool error, string message)
        {
            _action = action;
            _success = error;
            _message = message;
        }

        public override string ToString()
        {
            return ("[" + _action + "] " + (_success ? "Success: " : "Error: ") + _message);
        }
    }
}
