using System;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void Login(UserMsg userMsg)
            {
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    { 
                        if (!user.Password.Equals(userMsg.Password))
                        {
                            Console.WriteLine("Error: Wrong password");
                            Net.SendMsg(comm.GetStream(), new Answer(false, "Wrong password"));
                        }
                        else if (user.Comm != null)
                        {
                            Console.WriteLine("User already connected");
                            Net.SendMsg(comm.GetStream(), new Answer(false, "User Already connected"));
                        }
                        else
                        {
                            Console.WriteLine("Success: Login succesfully");
                            user.Comm = comm;
                            currentUser = user;
                            Net.SendMsg(comm.GetStream(), new Answer(true, "Loged-in succesfully"));
                        }
                        return;
                    }
                }
                Console.WriteLine("Error: No user with that username");
                Net.SendMsg(comm.GetStream(), new Answer(false, "No user with that username"));
            }

            private void Register(UserMsg userMsg)
            {
                bool isValid = true;
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    {
                        Console.WriteLine("Error: An user with that username already exist");
                        Net.SendMsg(comm.GetStream(), new Answer(false, "An user with that username already exist"));
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    Console.WriteLine("Creation of the new user...");
                    currentUser = new User(userMsg, comm);
                    userList.Add(currentUser);
                    userList.Serialize();

                    Console.WriteLine("Success: New user added");
                    Net.SendMsg(comm.GetStream(), new Answer(true, "New user added"));
                }
            }

            public void Disconnect()
            {
                currentUser.Comm = null;
                currentUser.Topic = null;


                Console.WriteLine("User " + currentUser.Username + " succesfully disconnected");
                Net.SendMsg(comm.GetStream(), new Answer(true, "Disconnected succesfully."));

                currentUser = null;
            }
        }
    }
}
