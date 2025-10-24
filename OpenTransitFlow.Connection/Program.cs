// Program.cs
using System.Net.WebSockets;
using System.Text;
using OpenTransitFlow.Connection;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// small fix, to be removed later
builder.WebHost.UseUrls("http://0.0.0.0:5189");

app.UseWebSockets();
Server server = new Server();
app.Map("/ws", async context =>
{

    using var socket = await server.AcceptWebSocketAsync(context);
    var result = await server.ReadStreamToSyncDebug(socket, context);
    Console.WriteLine(result);
});

app.Run();
