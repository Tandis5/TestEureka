using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery;

namespace TestEureka
{
    internal class StupidBackgroundService : IHostedService
    {
        private bool _running = true;
        private readonly IServiceProvider _serviceProvider;

        public StupidBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = BackgroundTask();
            return Task.CompletedTask;
        }

        private async Task BackgroundTask()
        {
            IDiscoveryClient discoveryClient = _serviceProvider.GetRequiredService<IDiscoveryClient>();

            while (_running)
            {
                Console.WriteLine("Hello");
                Console.WriteLine($"{discoveryClient.Description}");
                await Task.Delay(2000);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;
            return Task.CompletedTask;
        }
    }
}
