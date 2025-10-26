using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

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

        /// <summary>
        /// Serialize input Stream to Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="socket"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal async Task<T> DeserializeFromStream<T>(WebSocket socket, HttpContext context)
        {
            var jsonString = await ReadStreamToSync(socket, context);
            return JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("Could not create DTO!");
        }

#if DEBUG
        /// <summary>
        /// DEBUG Method to read stream into string.
        /// </summary>
        /// <param name="socket">Current Socket with connection</param>
        /// <returns>Returns received string</returns>
        internal async Task<string> ReadStreamToSyncDebug(WebSocket socket, HttpContext context)
        {
            return await ReadStreamToSync(socket, context);
        }
#endif
        
        private async Task<string> ReadStreamToSync(WebSocket socket, HttpContext context)
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


        private async Task<int> SendStream(WebSocket socket, HttpContext context, string message)
        {
            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
            await socket.SendAsync(messageBuffer, WebSocketMessageType.Text, true, context.RequestAborted);
            return messageBuffer.Length;
        }
        /// <summary>
        /// Method that can be used to send truncs of data through the websocket.
        /// </summary>
        /// <returns>Message buffer length</returns>
        public async Task<int> SendAsync<T>(WebSocket socket, HttpContext context, T obj)
        {
            string message = JsonSerializer.Serialize(obj);
            return await SendStream(socket, context, message);
        }
    }
}
