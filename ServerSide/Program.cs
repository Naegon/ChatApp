namespace ServerSide
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server(8976);
            server.Start();
        }
    }
}
