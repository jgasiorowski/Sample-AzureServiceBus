using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Worker
{
    internal class GracefulStopperHostedService : IHostedService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<GracefulStopperHostedService> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public GracefulStopperHostedService(IServiceProvider provider, ILogger<GracefulStopperHostedService> logger, IHostApplicationLifetime applicationLifetime)
        {
            _provider = provider;
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Yield();

            var secondsToTriggerStop = 8;
            _logger.LogInformation("Application will attempt to stop in {seconds}", secondsToTriggerStop);
            await Task.Delay(TimeSpan.FromSeconds(secondsToTriggerStop), cancellationToken);

            var hostedServices = _provider.GetServices<IHostedService>();

            foreach (var service in hostedServices.Where(s => s != this))
            {
                _logger.LogInformation("Stopping {serviceType} service", service.GetType().FullName);
                await service.StopAsync(cancellationToken);
                _logger.LogInformation("Stoped {serviceType} service", service.GetType().FullName);
            }

            _logger.LogInformation("Application will stop");
            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
