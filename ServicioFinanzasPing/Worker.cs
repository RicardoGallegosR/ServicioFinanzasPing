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
                var horaActual = DateTime.Now.TimeOfDay;
                var inicio = new TimeSpan(7, 0, 0);  
                var fin = new TimeSpan(20, 0, 0);    

                if (horaActual >= inicio && horaActual <= fin) {
                    _logger.LogInformation("Ejecutando monitoreo a las {time}", DateTimeOffset.Now);

                    try {
                        await _monitor.Monitor();
                    } catch (Exception ex) {
                        _logger.LogError(ex, "Error durante el monitoreo.");
                    }
                } else {
                    _logger.LogInformation("Fuera del horario de monitoreo. Hora actual: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
