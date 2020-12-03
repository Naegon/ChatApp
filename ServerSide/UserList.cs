using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication;

namespace ServerSide
{
    [Serializable]
    public class UserList
    {
        public List<User> all;
        
        public UserList()
        {
            all = new List<User>();
        }

        public void Add(User user)
        {
            all.Add(user);
        }

        public void Serialize()
        {
            Stream stream = File.Open("UserList.txt", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(stream, this);
            stream.Close();
        }

        public static UserList Deserialize()
        {
            UserList userList = new UserList();
            Stream stream = File.Open("UserList.txt", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            userList = (UserList)bf.Deserialize(stream);
            stream.Close();
            return userList;
        }

        public void Print()
        {
            if (all.Count > 0) all.ForEach(User => Console.WriteLine(User));
            else Console.WriteLine("No user yet");
        }
    }
}
