using System;

namespace ClientSide
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(1000);


            Client client = new Client("127.0.0.1", 8976);

            client.Menu();
        }
    }
}
