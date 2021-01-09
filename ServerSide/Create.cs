using System.Collections.Generic;
using Communication;

namespace ServerSide
{
    public static class Create
    {
        public static void Users()
        {
            var userList = new UserList
                    {
                        new User("Bob", "qwe"),
                        new User("Seb", "qwe"),
                        new User("Léo", "qwe"),
                        new User("Pam", "qwe")
                    };

            userList.Serialize();
        }

        public static void Topics()
        {
            var chatMusique = new List<Chat>
                    {
                        new Chat("Bob", "Hi Seb"),
                        new Chat("Seb", "Oh hey, morning Bob !"),
                        new Chat("Léo", "Who else is here?"),
                        new Chat("Pam", "There's me!"),
                        new Chat("Seb", "Hi Pam!")
                    };

            var chatSport = new List<Chat>
                    {
                        new Chat("Léo", "Tomorrow is biathlon!"),
                        new Chat("Pam", "Oh cool ! At what time?"),
                        new Chat("Léo", "At 14PM"),
                        new Chat("Léo", "Ahem no, 15PM it is!"),
                        new Chat("Pam", "Wonderful, well i'll be there!")
                    };

            var topicList = new TopicList
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
