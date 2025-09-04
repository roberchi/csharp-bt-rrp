using BtService.Models;
using BtService.Services;
using Microsoft.Extensions.Logging;

namespace BtService.Tests;

public class BluetoothManagerTests
{
    [Fact]
    public async Task ScanAsync_ReturnsDevicesFromAllClients()
    {
        var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<BluetoothManager>();
        var clients = new IBluetoothClient[] { new BleClient(), new ClassicClient() };
        var manager = new BluetoothManager(clients, logger);

        var devices = (await manager.ScanAsync()).ToList();

        Assert.Contains(devices, d => d.Name == "BleDevice");
        Assert.Contains(devices, d => d.Name == "ClassicDevice");
    }
}
