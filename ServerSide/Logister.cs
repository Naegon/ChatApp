using System;
using System.Linq;
using Communication;

namespace ServerSide
{
    public partial class Server
    {
        private partial class Receiver
        {
            private void Login(UserMsg userMsg)
            {
                foreach (var user in _userList.Where(user => user.Username.Equals(userMsg.Username)))
                {
                    if (!user.Password.Equals(userMsg.Password))
                    {
                        Console.WriteLine("Error: Wrong password");
                        Net.SendMsg(_comm.GetStream(), new Answer(false, "Wrong password"));
                    }
                    else if (user.Comm != null)
                    {
                        Console.WriteLine("User already connected");
                        Net.SendMsg(_comm.GetStream(), new Answer(false, "User Already connected"));
                    }
                    else
                    {
                        Console.WriteLine("Success: Login successfully");
                        user.Comm = _comm;
                        _currentUser = user;
                        Net.SendMsg(_comm.GetStream(), new Answer(true, "Logged-in successfully"));
                    }
                    return;
                }
                Console.WriteLine("Error: No user with that username");
                Net.SendMsg(_comm.GetStream(), new Answer(false, "No user with that username"));
            }

            private void Register(UserMsg userMsg)
            {
                var isValid = true;
                foreach (var unused in _userList.Where(user => user.Username.Equals(userMsg.Username)))
                {
                    Console.WriteLine("Error: An user with that username already exist");
                    Net.SendMsg(_comm.GetStream(), new Answer(false, "An user with that username already exist"));
                    isValid = false;
                }

                if (!isValid) return;
                Console.WriteLine("Creation of the new user...");
                _currentUser = new User(userMsg, _comm);
                _userList.Add(_currentUser);
                _userList.Serialize();

                Console.WriteLine("Success: New user added");
                Net.SendMsg(_comm.GetStream(), new Answer(true, "New user added"));
            }

            private void Disconnect()
            {
                _currentUser.Comm = null;
                _currentUser.Topic = null;


                Console.WriteLine("User " + _currentUser.Username + " successfully disconnected");
                Net.SendMsg(_comm.GetStream(), new Answer(true, "Disconnected successfully."));

                _currentUser = null;
            }
        }
    }
}
