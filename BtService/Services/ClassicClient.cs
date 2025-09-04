using BtService.Models;

namespace BtService.Services;

public class ClassicClient : IBluetoothClient
{
    public Task<IEnumerable<BluetoothDevice>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var devices = new List<BluetoothDevice>
        {
            new("ClassicDevice", "AA:BB:CC:DD:EE:FF")
        };
        return Task.FromResult<IEnumerable<BluetoothDevice>>(devices);
    }
}
