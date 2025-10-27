using OpenTransitFlow.Connection.Logic.Socket;
using OpenTransitFlow.Connection.Graph;
using QuikGraph;

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
                var tracksDTO = server.DeserializeFromStream<List<BaseTrackJson>>(socket, context).Result;
                
                var graph = GraphFactory.Create(tracksDTO);
                GraphPngRenderer.Renderer(graph);
            });
        }
    }
}