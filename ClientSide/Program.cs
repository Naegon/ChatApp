namespace ClientSide
{
    internal static class Program
    {
        private static void Main()
        {
            System.Threading.Thread.Sleep(1000);
            var client = new Client("127.0.0.1", 8976);

            client.Menu();
        }
    }
}
