using OpenTransitFlow.Connection;

class Program
{
    static void Main(string[] args)
    {
        Server server = new Server();
        server.StartAndListen().Wait();
    }
}