using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    [Serializable]
    public class UserList : List<User>
    {        
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

        public override string ToString()
        {
            if (Count > 0)
            {
                string outStr = "User list:\n";
                ForEach(user => outStr += "  - " + user + "\n");
                return outStr;
            }
            else return "No user yet";
        }
    }
}
