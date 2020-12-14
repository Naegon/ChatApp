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
                        new Chat("Bob", "Salut Seb"),
                        new Chat("Seb", "Oh tient, salut bob !"),
                        new Chat("Léo", "Qui d'autre est là ?"),
                        new Chat("Pam", "Il y a moi !"),
                        new Chat("Seb", "Salut Pam !"),
                    };

            List<Chat> chatSport = new List<Chat>
                    {
                        new Chat("Léo", "Demain c'est biathlon !"),
                        new Chat("Pam", "Oh cool ! Quelle heure ?"),
                        new Chat("Léo", "14H30"),
                        new Chat("Léo", "Euh non, 15H !"),
                        new Chat("Pam", "Super, c'est noté !"),
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
