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
