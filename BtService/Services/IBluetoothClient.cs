using BtService.Models;

namespace BtService.Services;

/// <summary>
/// Defines the operations required by a bluetooth client implementation.
/// </summary>
public interface IBluetoothClient
{
    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    event EventHandler<bool>? ConnectionStateChanged;

    /// <summary>
    /// Occurs when data is received from the device.
    /// </summary>
    event EventHandler<byte[]>? DataReceived;

    /// <summary>
    /// Indicates whether the client is currently connected to a device.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Scans for available bluetooth devices.
    /// </summary>
    Task<IEnumerable<BluetoothDevice>> Scan(CancellationToken cancellationToken = default);

    /// <summary>
    /// Connects to the specified bluetooth device.
    /// </summary>
    Task Connect(BluetoothDevice device, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the currently connected device.
    /// </summary>
    Task Disconnect(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reads data from the connected device.
    /// </summary>
    Task<byte[]> Read(CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes data to the connected device.
    /// </summary>
    Task Write(byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the services exposed by the connected device.
    /// </summary>
    Task<IEnumerable<BluetoothService>> GetServices(CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts streaming data from the connected device.
    /// </summary>
    Task StartStream(CancellationToken cancellationToken = default);
}
