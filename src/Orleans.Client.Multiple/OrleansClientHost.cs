using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Orleans.Client.Multiple;

public sealed class OrleansClientHost
{
    public string Name { get; }
    public IHost Host { get; }

    public OrleansClientHost(string name, Action<IClientBuilder> configureClient)
    {
        Name = name;
        Host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton(this);
            })
            .UseOrleansClient(configureClient)
            .Build();
    }

    public async Task StartAsync() => await Host.StartAsync();
    public async Task StopAsync() => await Host.StopAsync();

    public IClusterClient Client => Host.Services.GetRequiredService<IClusterClient>();
}