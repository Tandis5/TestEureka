using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using Steeltoe.Extensions.Configuration.ConfigServer;
using TestEureka;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);

hostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
{
    IHostEnvironment hostingEnvironment = hostingContext.HostingEnvironment;

    ConfigServerClientSettings clientSettings = new ConfigServerClientSettings()
    {
        Name = "TestEureka",
        Environment = hostingEnvironment.EnvironmentName,
        FailFast = true,
        RetryEnabled = true,
        RetryInitialInterval = 4000,
        RetryMultiplier = 2,
        RetryMaxInterval = 60000,
        RetryAttempts = 6
    };

    config
        .AddJsonFile($"appsettings.json", optional: true)
        .AddConfigServer(clientSettings)
        .AddEnvironmentVariables()
        .Build();
});

hostBuilder.ConfigureServices((hostContext, services) =>
{
    services.AddOptions();
    services.AddDiscoveryClient();
    services.AddHostedService<StupidBackgroundService>();
});

using IHost host = hostBuilder.Build();
await host.RunAsync();