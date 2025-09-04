using BtService.Models;
using System.Net.WebSockets;
using System.Text;

namespace BtService.Services;

public class BluetoothManager
{
    private readonly IEnumerable<IBluetoothClient> _clients;
    private readonly ILogger<BluetoothManager> _logger;

    public BluetoothManager(IEnumerable<IBluetoothClient> clients, ILogger<BluetoothManager> logger)
    {
        _clients = clients;
        _logger = logger;
    }

    public async Task<IEnumerable<BluetoothDevice>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var devices = new List<BluetoothDevice>();
        foreach (var client in _clients)
        {
            devices.AddRange(await client.ScanAsync(cancellationToken));
        }
        return devices;
    }

    public string GetStatus() => "running";

    public async Task HandleWebSocketAsync(WebSocket socket, CancellationToken cancellationToken = default)
    {
        var message = Encoding.UTF8.GetBytes("{\"status\":\"connected\"}");
        var segment = new ArraySegment<byte>(message);
        await socket.SendAsync(segment, WebSocketMessageType.Text, true, cancellationToken);
        // simple echo loop
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
            }
            else
            {
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logger.LogInformation("WS message: {Message}", msg);
            }
        }
    }
}
