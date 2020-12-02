using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    public class Net
    {
        public static void sendMsg(Stream s, User user)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, user);
        }

        public static User rcvMsg(Stream s)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (User)bf.Deserialize(s);
        }
    }
}
