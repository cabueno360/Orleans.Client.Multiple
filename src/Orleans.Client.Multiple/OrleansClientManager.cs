using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Client.Multiple;

public interface IOrleansClientManager
{
    void AddClient(string name, Action<IClientBuilder> configureClient);
    Task StartAllAsync();
    Task StopAllAsync();
    IClusterClient GetClient(string name);
}

public sealed class OrleansClientManager : IOrleansClientManager
{
    private readonly Dictionary<string, OrleansClientHost> _clients = new();

    public void AddClient(string name, Action<IClientBuilder> configureClient)
    {
        var clientHost = new OrleansClientHost(name, configureClient);
        _clients[name] = clientHost;
    }

    public async Task StartAllAsync()
    {
        foreach (var clientHost in _clients.Values)
        {
            await clientHost.StartAsync();
        }
    }

    public async Task StopAllAsync()
    {
        foreach (var clientHost in _clients.Values)
        {
            await clientHost.StopAsync();
        }
    }

    public IClusterClient GetClient(string name)
    {
        if (_clients.TryGetValue(name, out var clientHost))
        {
            return clientHost.Client;
        }
        throw new KeyNotFoundException($"Cliente Orleans com o nome '{name}' não foi encontrado.");
    }
}

