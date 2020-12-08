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
            string _out = "Topic list:\n";
            foreach (string title in _titles)
            {
                _out += "  - " + title + "\n";
            }
            return _out;
        }
    }

    [Serializable]
    public class Chat : Message
    {
        private string _sender;
        private string _content;

        public string Sender { get => _sender; }
        public string Content { get => _content; }

        public Chat(string sender, string content)
        {
            _sender = sender;
            _content = content;
        }

        public override string ToString()
        {
            return "\n[" + _sender + "] " + _content;
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
