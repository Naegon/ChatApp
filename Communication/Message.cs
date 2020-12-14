using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Message
    {
        public Message() { }
    }

    [Serializable]
    public class Topic : Message
    {
        private string _title;
        private List<Chat> _chats;

        public string Title { get => _title; }
        public List<Chat> Chats { get => _chats; }

        public Topic()
        {
            _title = "";
            _chats = new List<Chat>();
        }

        public Topic(string title)
        {
            _title = title;
            _chats = new List<Chat>();
        }

        public Topic(string title, List<Chat> chats)
        {
            _title = title;
            _chats = chats;
        }

        public override string ToString()
        {
            string strOut = _title + " (" + _chats.Count + " messages)\n";
            _chats.ForEach(chat => strOut += chat + "\n");
            return strOut;
        }
    }

    [Serializable]
    public class TopicListMsg : Message
    {
        private readonly List<string> _titles;
        public List<string> Titles { get => _titles; }

        public TopicListMsg(TopicList topicList)
        {
            _titles = new List<string>();
            foreach (Topic topic in topicList)
            {
                _titles.Add(topic.Title);
            }
        }

        public override string ToString()
        {
            string result = "Topic list:\n";
            foreach (string title in _titles)
            {
                result += "  - " + title + "\n";
            }
            return result;
        }
    }

    [Serializable]
    public class UserListMsg : Message
    {
        private readonly List<string> _usernames;
        public List<string> Usernames { get => _usernames; }

        public UserListMsg()
        {
            _usernames = new List<string>();
        }

        public UserListMsg(UserList userList, User currentUser)
        {
            _usernames = new List<string>();
            foreach (User user in userList)
            {
                if (user.Username != currentUser.Username)
                {
                    _usernames.Add(user.Username);
                }
            }
        }

        public override string ToString()
        {
            string result = "User list:";
            var i = 1;
            if (_usernames.Count <= 0) return "No connected user yet. Please try later.";

            foreach (string username in _usernames)
            {
                result += "\n" + i + ". " + username;
                i++;
            }
            return result;
        }
    }

    [Serializable]
    public class Chat : Message
    {
        private readonly string _sender;
        private readonly string _content;

        public string Sender { get => _sender; }
        public string Content { get => _content; }

        public Chat(string sender, string content)
        {
            _sender = sender;
            _content = content;
        }

        public override string ToString()
        {
            return "[" + _sender + "] " + _content;
        }
    }

    [Serializable]
    public class Answer : Message
    {
        private readonly bool _success;
        private readonly string _message;

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
