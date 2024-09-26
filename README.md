
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
dotnet add package Orleans.Client.Multiple --version 8.2.0.0
```

Or you can install it via the NuGet UI in Visual Studio.

### Usage Example

Below is an example of how to use `Orleans.Client.Multiple` in your project:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Orlean.Client.Multiple;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configBuilder = builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

var clientManager = new OrleansClientManager();

clientManager.AddClient("Silo1", client =>
{
    client
        .UseLocalhostClustering()
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "ServiceId1";
        });
});


clientManager.AddClient("Silo2", client =>
{
    client
       .UseRedisClustering(opt =>
        {
            opt.ConfigurationOptions = redisConfiguration;
        })
        .Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "dev";
            options.ServiceId = "ServiceId2";
        });
});

builder.Services.AddSingleton<IOrleansClientManager>(clientManager);


// You can inject in an method of minimal api or any another class that you need as bellow

public static async Task<IResult> SampleMethodUsingMultipe([FromServices] IOrleansClientManager clientManager, string id)
{
   
    var silo1Client = clientManager.GetClient("Silo1");
    var silo2Client = clientManager.GetClient("Silo2");

    var grain1 = silo1Client.GetGrain<IGrain1>(id);
    var grain2 = silo2Client.GetGrain<IGrain2>(id);

    // Now you can use your grains from different Silos

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
