using System.Net.WebSockets;
using System.Text;

namespace OpenTransitFlow.Connection
{
    public class Server
    {

        /// <summary>
        /// Method to set socket to accept websocket requeusts.
        /// </summary>
        /// <param name="context">Takes in current HttpContext containing requeust</param>
        /// <returns>Socket for requeusted connection</returns>
        /// <exception cref="Exception">If requeust is not for websocket, it will throw a Exception</exception>
        internal async Task<WebSocket> AcceptWebSocketAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                throw new Exception("Not a requeust to the socket");
            }

            return await context.WebSockets.AcceptWebSocketAsync();
        }

#if DEBUG
        /// <summary>
        /// DEBUG Method to read stream into string.
        /// </summary>
        /// <param name="socket">Current Socket with connection</param>
        /// <returns>Returns received string</returns>
        internal async Task<string> ReadStreamToSyncDebug(WebSocket socket, HttpContext context)
        {
            byte[] buffer = new byte[64 * 1024];
            var sb = new StringBuilder();

            while (true)
            {
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, context.RequestAborted);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing socket", context.RequestAborted);
                    break;
                }

                sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.EndOfMessage)
                {
                    string message = sb.ToString();
                    sb.Clear();
                    byte[] ack = Encoding.UTF8.GetBytes("{\"ok\":true}");
                    await socket.SendAsync(ack, WebSocketMessageType.Text, true, context.RequestAborted);
                    return message;
                }
            }

            // If this point is reached nothing was there to be printed, so empty strings are returned;
            return String.Empty;
        }
#else
        [Obsolete]
        internal async Task<string> ReadStreamToSyncDebug(WebSocket socket, HttpContext context)
        {
            throw new Exception("This method only exisits in DEBUG mode, dont use in production!!!")
        }
#endif
    }
}
