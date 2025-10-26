// Program.cs
using OpenTransitFlow.Connection.Endpoints;
using OpenTransitFlow.Connection.Logic.Socket;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// small fix, to be removed later
builder.WebHost.UseUrls("http://0.0.0.0:5189");

app.UseWebSockets();
Server server = new Server();
app.MapWebSocket(server);

app.Run();
