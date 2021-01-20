using System;
using System.Collections.Generic;
using System.Linq;

namespace Communication
{
    [Serializable]
    public class Message { }

    [Serializable]
    public class Topic : Message
    {
        public string Title { get; }
        public List<Chat> Chats { get; }

        public Topic()
        {
            Title = "";
            Chats = new List<Chat>();
        }

        public Topic(string title)
        {
            Title = title;
            Chats = new List<Chat>();
        }

        public Topic(string title, List<Chat> chats)
        {
            Title = title;
            Chats = chats;
        }

        public override string ToString()
        {
            var strOut = Title + " (" + Chats.Count + " messages)\n";
            Chats.ForEach(chat => strOut += chat + "\n");
            return strOut;
        }
    }

    [Serializable]
    public class TopicListMsg : Message
    {
        public List<string> Titles { get; }

        public TopicListMsg(TopicList topicList)
        {
            Titles = new List<string>();
            foreach (var topic in topicList)
            {
                Titles.Add(topic.Title);
            }
        }

        public override string ToString()
        {
            return Titles.Aggregate("Topic list:\n", (current, title) => current + "  - " + title + "\n");
        }
    }

    [Serializable]
    public class UserListMsg : Message
    {
        public List<string> Usernames { get; }

        public UserListMsg()
        {
            Usernames = new List<string>();
        }

        public UserListMsg(UserList userList, User currentUser)
        {
            Usernames = new List<string>();
            foreach (var user in userList.Where(user => user.Username != currentUser.Username))
            {
                Usernames.Add(user.Username);
            }
        }

        public override string ToString()
        {
            var result = "User list:";
            var i = 1;
            if (Usernames.Count <= 0) return "No connected user yet. Please try later.";

            foreach (var username in Usernames)
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
        public string Sender { get; }
        public string Content { get; }

        public Chat(string sender, string content)
        {
            Sender = sender;
            Content = content;
        }

        public override string ToString()
        {
            return "[" + Sender + "] " + Content;
        }
    }

    [Serializable]
    public class Answer : Message
    {
        public bool Success { get; }
        public string Message { get; }


        public Answer(bool error, string message)
        {
            Success = error;
            Message = message;
        }

        public override string ToString()
        {
            return (Success ? "Success: " : "Error: ") + Message;
        }
    }
}
