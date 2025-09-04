using BtService.Models;
using BtService.Services;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration is loaded from appsettings.json, environment variables and command line by default
builder.Services.AddSingleton<IBluetoothClient, BleClient>();
builder.Services.AddSingleton<IBluetoothClient, ClassicClient>();
builder.Services.AddSingleton<BluetoothManager>();

builder.Services.AddLogging();

var app = builder.Build();

app.MapGet("/bluetooth/scan", async (BluetoothManager manager) => await manager.ScanAsync());
app.MapGet("/bluetooth/status", (BluetoothManager manager) => new { status = manager.GetStatus() });

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
 <title>Bluetooth Manager</title>
 <link rel=\"stylesheet\" href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css\" />
</head>
<body class=\"p-3\">
 <h1>Bluetooth Manager</h1>
 <p>Simple web interface placeholder.</p>
</body>
</html>
""", "text/html"));

app.MapGet("/ws/stream", async (HttpContext context, BluetoothManager manager) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        await manager.HandleWebSocketAsync(socket);
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.Run();
