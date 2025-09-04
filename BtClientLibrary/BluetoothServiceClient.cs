using System.Net.WebSockets;
using System.Net.Http.Json;
using System.Text;
using BtClientLibrary.Models;

namespace BtClientLibrary;

public class BluetoothServiceClient
{
    private readonly HttpClient _httpClient;
    private ClientWebSocket? _webSocket;

    public event EventHandler<string>? StreamReceived;

    public BluetoothServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BluetoothDevice>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var devices = await _httpClient.GetFromJsonAsync<IEnumerable<BluetoothDevice>>("/bluetooth/scan", cancellationToken);
        return devices ?? Enumerable.Empty<BluetoothDevice>();
    }

    public async Task ConnectStreamAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        _webSocket = new ClientWebSocket();
        await _webSocket.ConnectAsync(uri, cancellationToken);
        _ = ReceiveLoop(_webSocket, cancellationToken);
    }

    private async Task ReceiveLoop(ClientWebSocket socket, CancellationToken cancellationToken)
    {
        var buffer = new byte[1024];
        while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
            }
            else
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                StreamReceived?.Invoke(this, message);
            }
        }
    }
}
