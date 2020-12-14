﻿using System;
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
                            Net.sendMsg(comm.GetStream(), new Answer(false, "Wrong password"));
                        }
                        else if (user.Comm != null)
                        {
                            Console.WriteLine("User Already connected");
                            Net.sendMsg(comm.GetStream(), new Answer(false, "User Already connected"));
                        }
                        else
                        {
                            Console.WriteLine("Success: Login succesfully");
                            user.Comm = comm;
                            _currentUser = user;
                            Net.sendMsg(comm.GetStream(), new Answer(true, "Loged-in succesfully"));
                        }
                        return;
                    }
                }
                Console.WriteLine("Error: No user with that username");
                Net.sendMsg(comm.GetStream(), new Answer(false, "No user with that username"));
            }

            private void Register(UserMsg userMsg)
            {
                bool isValid = true;
                foreach (User user in userList)
                {
                    if (user.Username.Equals(userMsg.Username))
                    {
                        Console.WriteLine("Error: An user with that username already exist");
                        Net.sendMsg(comm.GetStream(), new Answer(false, "An user with that username already exist"));
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    Console.WriteLine("Creation of the new user...");
                    _currentUser = new User(userMsg, comm);
                    userList.Add(_currentUser);
                    userList.Serialize();

                    Console.WriteLine("Success: New user added");
                    Net.sendMsg(comm.GetStream(), new Answer(true, "New user added"));
                }
            }

            public void Disconnect()
            {
                _currentUser.Comm = null;
                _currentUser.Topic = null;


                Console.WriteLine("User " + _currentUser.Username + " succesfully disconnected");
                Net.sendMsg(comm.GetStream(), new Answer(true, "Disconnected succesfully."));

                _currentUser = null;
            }
        }
    }
}
