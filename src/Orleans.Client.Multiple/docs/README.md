
# Orleans.Client.Multiple

Orleans.Client.Multiple is a library that enables support for multiple Orleans clients within the same .NET application. This allows you to connect to multiple Orleans clusters or services, each with different configurations (such as different Service IDs or Cluster IDs), using Microsoft Orleans 8.2.0.

## Features

- **Multiple Orleans Clients**: Easily manage and connect multiple Orleans clients within a single application.
- **Customizable Configurations**: Each client can have its own configuration (such as different Service IDs or Cluster IDs).
- **Seamless Orleans Integration**: Built on top of Microsoft Orleans 8.2.0.

## Getting Started

### Installation

You can install this package via NuGet Package Manager Console:

```bash
dotnet add package Orleans.Client.Multiple --version 8.2.0
```

Or you can install it via the NuGet UI in Visual Studio.

### Usage Example

Below is an example of how to use `Orleans.Client.Multiple` in your project:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Orlean.Client.Multiple;

class Program
{
    static async Task Main(string[] args)
    {
        var clientManager = new OrleansClientManager();

        clientManager.AddClient("Cluster1", clientBuilder =>
        {
            clientBuilder.UseAdoNetClustering(options =>
            {
                options.ConnectionString = "Server=localhost;Database=OrleansCluster1;User Id=yourUser;Password=yourPassword;";
                options.Invariant = "System.Data.SqlClient";
            });
        });

        clientManager.AddClient("Cluster2", clientBuilder =>
        {
            clientBuilder.UseLocalhostClustering(serviceId: "YourServiceId", clusterId: "YourClusterId");
        });

        await clientManager.StartAllAsync();

        var client1 = clientManager.GetClient("Cluster1");
        var client2 = clientManager.GetClient("Cluster2");

        // Use the clients to interact with Orleans clusters...
        
        await clientManager.StopAllAsync();
    }
}
```

In this example:
1. We create an instance of `OrleansClientManager`.
2. We add two Orleans clients, each connecting to a different Orleans cluster with its own configuration.
3. We start all clients, retrieve them by name, and then use them to interact with their respective Orleans clusters.
4. Finally, we stop all clients.

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request if you want to contribute.

## License

This project is licensed under the MIT License.
