using System;

namespace ClientSide
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(3000);


            Client client = new Client("127.0.0.1", 8976);

            Console.WriteLine("Welcome!");
            Console.WriteLine("1. Login \n2. Register \n3. Quit");

            String choice;
            do
            {
                Console.Write("\nPlease choose an option: ");
                choice = Console.ReadLine();
            } while (!choice.Equals("1") && !choice.Equals("2") && !choice.Equals("3"));

            switch (choice)
            {
                case ("1"):
                    Console.WriteLine("Login \n");
                    break;
                case ("2"):
                    Console.WriteLine("Register \n");
                    client.Register();
                    break;
                case ("3"):
                    Console.WriteLine("Quit");
                    break;
                default:
                    Console.WriteLine("There has been an error");
                    break;
            }
        }
    }
}
