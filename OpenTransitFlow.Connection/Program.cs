// Program.cs
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// small fix, to be removed later
builder.WebHost.UseUrls("http://0.0.0.0:5189");

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();

    var buffer = new byte[64 * 1024];
    var sb = new StringBuilder();

    while (true)
    {
        var result = await socket.ReceiveAsync(buffer, context.RequestAborted);
        if (result.MessageType == WebSocketMessageType.Close)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", context.RequestAborted);
            break;
        }

        sb.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

        if (result.EndOfMessage)
        {
            var message = sb.ToString();
            sb.Clear();
            Console.WriteLine($"[WS] Received: {message}");

            var ack = Encoding.UTF8.GetBytes("{\"ok\":true}");
            await socket.SendAsync(ack, WebSocketMessageType.Text, true, context.RequestAborted);
        }
    }
});

app.Run();
