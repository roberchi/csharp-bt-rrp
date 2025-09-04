using BtService.Models;

namespace BtService.Services;

public interface IBluetoothClient
{
    Task<IEnumerable<BluetoothDevice>> ScanAsync(CancellationToken cancellationToken = default);
}
