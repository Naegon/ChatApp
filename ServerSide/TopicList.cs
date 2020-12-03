using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication;

namespace ServerSide
{
    [Serializable]
    public class TopicList
    {
        public List<Topic> all;

        public TopicList()
        {
            all = new List<Topic>();
        }

        public void Add(Topic topic)
        {
            all.Add(topic);
        }

        public void Serialize()
        {
            Stream stream = File.Open("TopicList.txt", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, this);
            stream.Close();
        }

        public static TopicList Deserialize()
        {
            TopicList topicList = new TopicList();
            Stream stream = File.Open("TopicList.txt", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            topicList = (TopicList)bf.Deserialize(stream);
            stream.Close();
            return topicList;
        }

        public void Print()
        {
            if (all.Count > 0) all.ForEach(Topic => Console.WriteLine(Topic));
            else Console.WriteLine("No topic yet");
        }
    }
}
