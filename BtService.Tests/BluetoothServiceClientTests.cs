using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using BtClientLibrary;
using BtClientLibrary.Models;
using Xunit;

namespace BtService.Tests;

public class BluetoothServiceClientTests
{
    [Fact]
    public async Task ScanAsync_ParsesDevices()
    {
        var handler = new FakeHandler();
        var httpClient = new HttpClient(handler) { BaseAddress = new Uri("http://localhost") };
        var client = new BluetoothServiceClient(httpClient);

        handler.Response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(new [] { new BluetoothDevice("A", "1") })
        };

        var devices = await client.ScanAsync();

        Assert.Single(devices);
        Assert.Equal("A", devices.First().Name);
    }

    private class FakeHandler : HttpMessageHandler
    {
        public HttpResponseMessage? Response { get; set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(Response ?? new HttpResponseMessage(HttpStatusCode.NotFound));
    }
}
