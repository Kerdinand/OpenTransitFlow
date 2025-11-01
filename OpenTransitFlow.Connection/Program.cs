// Program.cs
using OpenTransitFlow.Connection.Endpoints;
using OpenTransitFlow.Infra.Graph;
using OpenTransitFlow.Connection.Logic.Socket;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// small fix, to be removed later
builder.WebHost.UseUrls("http://0.0.0.0:5189");

app.UseWebSockets();
Server server = new Server();
app.MapWebSocket(server);


var factory = new GraphFactory(true);
factory.AddNode("1", [0, 0]);
factory.AddNode("2", [1, 2]);
factory.AddEdge("E1", "1", "2", true);

var start = factory.GetNode("1");
var end = factory.GetNode("2");

factory.RunDijkstra(start, end);
factory.RunDijkstra(end, start);
app.Run();
