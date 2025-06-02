
using PingAdip.Monitoreo;
using System.Threading;

class Program {
    static async Task Main() {
        MonitoreoServicio _monitor = new MonitoreoServicio();
        await _monitor.Monitor();

    }
}
