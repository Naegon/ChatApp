using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{
    [Serializable]
    public class UserList : List<User>
    {
        public void Serialize()
        {
            foreach(var user in this) { user.Topic = ""; }

            Stream stream = File.Open("UserList.txt", FileMode.Create);
            var bf = new BinaryFormatter();
            bf.Serialize(stream, this);
            stream.Close();
        }

        public static UserList Deserialize()
        {
            Stream stream = File.Open("UserList.txt", FileMode.Open);
            var bf = new BinaryFormatter();

            var userList = (UserList)bf.Deserialize(stream);
            stream.Close();
            return userList;
        }

        public override string ToString()
        {
            return Count > 0 ? this.Aggregate("User list:\n", (current, user) => current + "  - " + user + "\n") : "No user yet";
        }
    }
}
