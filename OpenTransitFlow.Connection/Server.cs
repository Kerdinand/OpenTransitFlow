using System.Net;
using System.Net.Sockets;

namespace OpenTransitFlow.Connection
{
    /// <summary>
    /// Small wrapper to extract logic outside of Program.s
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Used to capture incoming Stream as of now.
        /// </summary>
        TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);

        /// <summary>
        /// Basic Function to start a new WebSocketConnection on default ip/port.
        /// </summary>
        /// <returns>Returns internal TcpListener</returns>
        public async Task<TcpListener> StartAndListen()
        {
            server.Start();
            Console.WriteLine("Server Started");
            await server.AcceptTcpClientAsync();
            Console.WriteLine($"Client connected from");
            return server;
        }



    }
}
