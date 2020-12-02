﻿using System;
using System.Net.Sockets;
using Communication;

namespace ClientSide
{
    public class Client
    {
        private TcpClient _comm;
        public TcpClient Comm { get => _comm; set => _comm = value; }

        public Client(string hostname, int port)
        {
            _comm = new TcpClient(hostname, port);
            Console.WriteLine("--> Connection established on " + hostname + ":" + port);
        }


        public void Register()
        {
            string username;
            string password;

            Console.WriteLine("Choose an username:");
            username = Console.ReadLine();

            Console.WriteLine("Choose a secur password:");
            password = GetPassword();

            Console.WriteLine("Sending data to server...");
            Net.sendMsg(Comm.GetStream(), new User(username, password));
            Console.WriteLine((Answer)Net.rcvMsg(Comm.GetStream()));
        }

        public string GetPassword()
        {
            string pwd = "";
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine("\n");
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Length > 0)
                    {
                        pwd.Remove(pwd.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (i.KeyChar != '\u0000') // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc
                {
                    pwd += i.KeyChar;
                    Console.Write("*");
                }
            }
            return pwd;
        }
    }
}