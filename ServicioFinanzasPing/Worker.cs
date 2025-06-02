using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PingAdip.Monitoreo;
using System.Threading;

namespace ServicioFinanzasPing {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly MonitoreoServicio _monitor;

        public Worker(ILogger<Worker> logger, MonitoreoServicio monitor) { 
            _logger = logger;
            _monitor = monitor;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Ejecutando monitoreo a las {time}", DateTimeOffset.Now);

                try {
                    await _monitor.Monitor();
                } catch (Exception ex) {
                    _logger.LogError(ex, "Error durante el monitoreo.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
