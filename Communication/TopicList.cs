using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    [Serializable]
    public class TopicList : List<Topic>
    {
        public void Serialize()
        {
            Stream stream = File.Open("TopicList.txt", FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(stream, this);
            stream.Close();
        }

        public static TopicList Deserialize()
        {
            Stream stream = File.Open("TopicList.txt", FileMode.Open);
            var bf = new BinaryFormatter();

            var topicList = (TopicList)bf.Deserialize(stream);
            stream.Close();
            return topicList;
        }

        public override string ToString()
        {
            return Count > 0 ? this.Aggregate("Topic list:\n", (current, topic) => current + "||-> " + topic + "\n") : "No topic yet";
        }
    }
}
