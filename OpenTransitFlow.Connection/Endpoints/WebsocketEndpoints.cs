using OpenTransitFlow.Connection.Logic.Socket;

namespace OpenTransitFlow.Connection.Endpoints
{
    public static class WebSocketEndpoints
    {
        public static void MapWebSocket(this WebApplication app, Server server)
        {
            app.Map("/ws", async context =>
            {
                using var socket = await server.AcceptWebSocketAsync(context);
                var result = await server.ReadStreamToSyncDebug(socket, context);
                var tracksDTO = await server.DeserializeFromStream<List<BaseTrackJson>>(socket, context);

                var graph = new GraphFactory().Create(tracksDTO);
            });
        }
    }
}