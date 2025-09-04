using BtService.Models;

namespace BtService.Services;

public class BleClient : IBluetoothClient
{
    public Task<IEnumerable<BluetoothDevice>> ScanAsync(CancellationToken cancellationToken = default)
    {
        var devices = new List<BluetoothDevice>
        {
            new("BleDevice", "00:11:22:33:44:55")
        };
        return Task.FromResult<IEnumerable<BluetoothDevice>>(devices);
    }
}
