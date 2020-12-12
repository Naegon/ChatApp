using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    public class Net
    {
        public enum Action
        {
            Login,
            Register,
            GetTopicList,
            GetUserList,
            PrivateMessage,
            CreateTopic,
            Join,
            Disconnect,
            Quit
        }

        //public const int Login = 0;
        //public const int Register = 1;
        //public const int GetTopicList = 2;
        //public const int GetUserList = 3;
        //public const int PrivateMessage = 4;
        //public const int CreateTopic = 5;
        //public const int Join = 6;
        //public const int Disconnect = 7;
        //public const int Quit = 8;

        public static void sendMsg(Stream s, Message message)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, message);
        }

        public static Message rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Message)bf.Deserialize(s);
        }
    }
}
