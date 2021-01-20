namespace ServerSide
{
    internal static class Program
    {
        private static void Main()
        {
            var server = new Server(8976);
            server.Start();
        }
    }
}
