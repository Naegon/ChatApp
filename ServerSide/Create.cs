using System.Collections.Generic;
using Communication;

namespace ServerSide
{
    public class Create
    {
        public static void Users()
        {
            UserList userList = new UserList
                    {
                        new User("Bob", "qwe"),
                        new User("Seb", "qwe"),
                        new User("Léo", "qwe"),
                        new User("Pam", "qwe")
                    };

            userList.Serialize();
        }

        public static void Topics(UserList userList)
        {
            List<Chat> chatMusique = new List<Chat>
                    {
                        new Chat(userList[0].Username, "Salut Seb"),
                        new Chat(userList[1].Username, "Oh tient, salut bob !"),
                        new Chat(userList[0].Username, "Qui d'autre est là ?"),
                        new Chat(userList[3].Username, "Il y a moi !"),
                        new Chat(userList[1].Username, "Salut Pam !"),
                    };

            List<Chat> chatSport = new List<Chat>
                    {
                        new Chat(userList[2].Username, "Demain c'est biathlon !"),
                        new Chat(userList[3].Username, "Oh cool ! Quelle heure ?"),
                        new Chat(userList[2].Username, "14H30"),
                        new Chat(userList[2].Username, "Euh non, 15H !"),
                        new Chat(userList[3].Username, "Super, c'est noté !"),
                    };

            TopicList topicList = new TopicList
                    {
                        new Topic("Music", chatMusique),
                        new Topic("Sport", chatSport),
                        new Topic("Art"),
                        new Topic("Cinema")
                    };

            topicList.Serialize();
        }
    }
}
