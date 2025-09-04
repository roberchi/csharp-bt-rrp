using BtService.Models;

namespace BtService.Services;

public class BleClient : IBluetoothClient
{
    public event EventHandler<bool>? ConnectionStateChanged;
    public event EventHandler<byte[]>? DataReceived;

    public bool IsConnected { get; private set; }

    public Task<IEnumerable<BluetoothDevice>> Scan(CancellationToken cancellationToken = default)
    {
        var devices = new List<BluetoothDevice>
        {
            new("BleDevice", "00:11:22:33:44:55")
        };
        return Task.FromResult<IEnumerable<BluetoothDevice>>(devices);
    }

    public Task Connect(BluetoothDevice device, CancellationToken cancellationToken = default)
    {
        IsConnected = true;
        ConnectionStateChanged?.Invoke(this, IsConnected);
        return Task.CompletedTask;
    }

    public Task Disconnect(CancellationToken cancellationToken = default)
    {
        IsConnected = false;
        ConnectionStateChanged?.Invoke(this, IsConnected);
        return Task.CompletedTask;
    }

    public Task<byte[]> Read(CancellationToken cancellationToken = default)
    {
        var data = Array.Empty<byte>();
        DataReceived?.Invoke(this, data);
        return Task.FromResult(data);
    }

    public Task Write(byte[] data, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<BluetoothService>> GetServices(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<BluetoothService>>(new List<BluetoothService>());
    }

    public Task StartStream(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

